using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Battle;
using System.Collections.Generic;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.StateMachines
{
    public class GameContextBattleState : ICoroutineState
    {
        private static float CHARACTERS_HORIZONTAL_OFFSET = 3f;
        private static float CENTER_RAYCAST_THRESHOLD = 6f;
        private static int CENTER_DOWN_RAYCAST_AMOUNT = 3;
        private static float CENTER_DOWN_RAYCAST_DISTANCE = 1f;
        private static float PARTY_HORIZONTAL_OFFSET = 0.4f;
        private static float PARTY_VERTICAL_OFFSET = 0.4f;

        private GameObject playerObject;
        private Camera playerCamera;
        private StringGameEvent actionMapSwitchEvent;
        private int centerCheckLayerMask;
        private bool goToMap = false;
        private RewardSystem rewardSystem;
        private PlayerMinionCompanionSpawner companionSpawner;
        private BattleSystem battleSystem;
        private GameObject[] enemyObjects;
        private int deathCount;
        private RewardData rewardData = null;
        private bool paused = false;

        #region ICoroutineState Implementation
        public IEnumerator EnterState()
        {
            deathCount = 0;
            actionMapSwitchEvent.Raise("PlayerBattle");
            ConfigureEnemyObjects();
            SpawnPlayerCompanions();
            // Init battle system
            CharacterBattleManager[] enemies = new CharacterBattleManager[enemyObjects.Length];
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i] = enemyObjects[i].GetComponent<CharacterBattleBehaviour>().battleManager;
            }
            battleSystem.InitBattle(playerObject.GetComponent<PlayerBattleBehaviour>().battleManager, enemies);
            // Move camera
            // Move characters
            MoveCharacters();
            GamedevsToolbox.Utils.Logger.Logger.Log("Entering battle state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            // Deactivate things
            companionSpawner.Despawn();
            battleSystem.EndBattle();
            // Yield end battle screen and give rewards
            actionMapSwitchEvent.Raise("UI");
            yield return rewardSystem.GetReward(rewardData);
            goToMap = false;
            yield return null;
        }

        public void Pause()
        {
            battleSystem.Pause();
            paused = true;
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
            battleSystem.Resume();
            paused = false;
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (!paused)
            {
                if (goToMap)
                {
                    resolve("Map");
                }
            }
            yield return battleSystem.Tick(Time.deltaTime);
        }
        #endregion

        #region Public Methods
        public void SetEnemyObjects(GameObject[] enemyObjects)
        {
            this.enemyObjects = enemyObjects;
            rewardData = enemyObjects[0].GetComponent<IRewardable>().GetReward();
        }

        public GameContextBattleState(GameObject playerObject, Camera playerCamera, StringGameEvent actionMapSwitchEvent, ScriptableIntReference bloodReference, ScriptableIntReference hardCurrencyReference, int centerCheckLayerMask, RewardUILibrary rewardUILibrary)
        {
            this.playerObject = playerObject;
            this.playerCamera = playerCamera;
            this.actionMapSwitchEvent = actionMapSwitchEvent;
            this.centerCheckLayerMask = centerCheckLayerMask;
            Player player = playerObject.GetComponent<PlayerDataBehaviour>().player;
            this.rewardSystem = new RewardSystem(player, bloodReference, hardCurrencyReference, rewardUILibrary);
            this.companionSpawner = new PlayerMinionCompanionSpawner(player);
            this.battleSystem = new BattleSystem();
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

            Vector3 originalCenter = GetCenter();
            Vector3 center = GetCenter(playerObject.transform.position, originalCenter) - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;

            companionSpawner.Spawn(center, playerObject);
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

        private Vector3 GetCenter(Vector3 originalPosition, Vector3 originalCenter)
        {
            ContactFilter2D raycastFilters = new ContactFilter2D();
            raycastFilters.layerMask = centerCheckLayerMask;
            raycastFilters.useTriggers = false;
            raycastFilters.useLayerMask = true;
            originalPosition.x = originalCenter.x;
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
            Turn(enemyObjects[0], !playerLookingRight);

            Vector3 originalCenter = GetCenter();
            playerObject.transform.position = GetCenter(playerObject.transform.position, originalCenter) - Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
            enemyObjects[0].transform.position = GetCenter(enemyObjects[0].transform.position, originalCenter) + Vector3.right * CHARACTERS_HORIZONTAL_OFFSET * modifier;
            for (int i = 1; i < enemyObjects.Length; ++i)
            {
                enemyObjects[1].transform.position = enemyObjects[0].transform.position + Vector3.left * PARTY_HORIZONTAL_OFFSET * modifier * i + Vector3.up * PARTY_VERTICAL_OFFSET * i;
            }
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