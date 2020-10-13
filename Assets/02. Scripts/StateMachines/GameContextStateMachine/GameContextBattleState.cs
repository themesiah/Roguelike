using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Battle;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using Laresistance.Core;

namespace Laresistance.StateMachines
{
    public class GameContextBattleState : ICoroutineState
    {
        private static float CHARACTERS_HORIZONTAL_OFFSET = 3f;
        private static float CENTER_RAYCAST_THRESHOLD = 6f;
        private static int CENTER_DOWN_RAYCAST_AMOUNT = 3;
        private static float CENTER_DOWN_RAYCAST_DISTANCE = 1f;

        private GameObject playerObject;
        private Camera playerCamera;
        private PlayerInput playerInput;
        private int centerCheckLayerMask;
        private bool goToMap = false;
        private RewardSystem rewardSystem;
        private PlayerMinionCompanionSpawner companionSpawner;

        private GameObject[] enemyObjects;
        private int deathCount;
        private RewardData rewardData = null;

        #region ICoroutineState Implementation
        public IEnumerator EnterState()
        {
            deathCount = 0;
            playerInput.SwitchCurrentActionMap("PlayerBattle");
            ObjectActivationAndDesactivation(true);
            ConfigureEnemyObjects();
            SpawnPlayerCompanions();
            // Move camera
            // Move characters
            MoveCharacters();
            Debug.Log("Entering battle state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            // Deactivate things
            ObjectActivationAndDesactivation(false);
            companionSpawner.Despawn();
            // Yield end battle screen and give rewards
            yield return rewardSystem.GetReward(rewardData);
            goToMap = false;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "FinishBattle")
            {
                goToMap = true;
            }
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (goToMap)
            {
                resolve("Map");
            }
            yield return null;
        }
        #endregion

        #region Public Methods
        public void SetEnemyObjects(GameObject[] enemyObjects)
        {
            this.enemyObjects = enemyObjects;
            rewardData = enemyObjects[0].GetComponent<IRewardable>().GetReward();
        }

        public GameContextBattleState(GameObject playerObject, Camera playerCamera, PlayerInput playerInput, ScriptableIntReference bloodReference, ScriptableIntReference hardCurrencyReference, int centerCheckLayerMask)
        {
            this.playerObject = playerObject;
            this.playerCamera = playerCamera;
            this.playerInput = playerInput;
            this.centerCheckLayerMask = centerCheckLayerMask;
            Player player = playerObject.GetComponent<PlayerBattleBehaviour>().player;
            this.rewardSystem = new RewardSystem(player, bloodReference, hardCurrencyReference);
            this.companionSpawner = new PlayerMinionCompanionSpawner(player);
        }
        #endregion

        #region Private methods
        private void ConfigureEnemyObjects()
        {
            foreach (var go in this.enemyObjects)
            {
                go.GetComponent<CharacterBattleBehaviour>().StatusManager.health.OnDeath += EnemyDeath;
            }
        }

        private void SpawnPlayerCompanions()
        {
            float modifier = -1f;
            if (playerObject.transform.position.x < enemyObjects[0].transform.position.x)
            {
                modifier = 1f;
            }

            Vector3 center = GetCenter(playerObject.transform.position) - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;

            companionSpawner.Spawn(center, playerObject);
        }

        private void ObjectActivationAndDesactivation(bool enter)
        {
            playerObject.GetComponent<CharacterBattleBehaviour>().enabled = enter;
            foreach (var go in enemyObjects)
            {
                go.GetComponent<CharacterBattleBehaviour>().enabled = enter;
            }
        }

        private void EnemyDeath(CharacterHealth health)
        {
            deathCount++;
            if (deathCount == enemyObjects.Length)
            {
                goToMap = true;
            }
        }

        private Vector3 GetCenter()
        {
            
            Vector3 tempCenter = (this.playerObject.transform.position + this.enemyObjects[0].transform.position) / 2f;
            float offset = CalculateOffset(tempCenter);
            tempCenter.x += offset;

            return tempCenter;
        }

        private float CalculateOffset(Vector3 center)
        {
            float offset = 0f;
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            bool horizontalHit = false;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            int hits = 0;

            // Left wall
            hits = Physics2D.Raycast(center + Vector3.up * 0.5f, Vector2.left, raycastFilters, results, CENTER_RAYCAST_THRESHOLD);
            if (hits > 0)
            {
                offset += (CENTER_RAYCAST_THRESHOLD - results[0].distance);
                horizontalHit = true;
            }

            // Right wall
            if (!horizontalHit) {
                hits = Physics2D.Raycast(center + Vector3.up * 0.5f, Vector2.right, raycastFilters, results, CENTER_RAYCAST_THRESHOLD);
                if (hits > 0)
                {
                    offset -= (CENTER_RAYCAST_THRESHOLD - results[0].distance);
                    horizontalHit = true;
                }
            }

            // Left fall
            if (!horizontalHit)
            {
                for (int i = 0; i < CENTER_DOWN_RAYCAST_AMOUNT; i++)
                {
                    hits = Physics2D.Raycast(center + Vector3.up * 0.5f + Vector3.left * CENTER_RAYCAST_THRESHOLD / (CENTER_DOWN_RAYCAST_AMOUNT - i), Vector2.down, raycastFilters, results, CENTER_DOWN_RAYCAST_DISTANCE);
                    if (hits == 0)
                    {
                        offset += CENTER_RAYCAST_THRESHOLD;
                        horizontalHit = true;
                        break;
                    }
                }
            }

            // Right fall
            if (!horizontalHit)
            {
                for (int i = 0; i < CENTER_DOWN_RAYCAST_AMOUNT; i++)
                {
                    hits = Physics2D.Raycast(center + Vector3.up * 0.5f + Vector3.right * CENTER_RAYCAST_THRESHOLD / (CENTER_DOWN_RAYCAST_AMOUNT - i), Vector2.down, raycastFilters, results, CENTER_DOWN_RAYCAST_DISTANCE);
                    if (hits == 0)
                    {
                        offset -= CENTER_RAYCAST_THRESHOLD;
                        break;
                    }
                }
            }


            return offset;
        }

        private Vector3 GetCenter(Vector3 originalPosition)
        {
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            originalPosition.x = GetCenter().x;
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            int hits = Physics2D.Raycast(originalPosition + Vector3.up * 0.5f, Vector2.down, raycastFilters, results);
            if (hits > 0)
            {
                originalPosition = results[0].point;
            }
            return originalPosition;
        }

        private void MoveCharacters()
        {
            bool playerLookingRight = false;
            float modifier = -1f;
            if (playerObject.transform.position.x < enemyObjects[0].transform.position.x)
            {
                playerLookingRight = true;
                modifier = 1f;
            }
            Turn(playerObject, playerLookingRight);
            foreach(var go in enemyObjects)
            {
                Turn(go, !playerLookingRight);
            }

            playerObject.transform.position = GetCenter(playerObject.transform.position) - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
            enemyObjects[0].transform.position = GetCenter(enemyObjects[0].transform.position) + Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
        }

        private void Turn(GameObject character, bool right)
        {
            Vector3 scale = character.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            if (!right)
            {
                scale.x *= -1;
            }
            character.transform.localScale = scale;
        }
        #endregion
    }
}