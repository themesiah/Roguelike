using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Battle;
using UnityEngine.InputSystem;

namespace Laresistance.StateMachines
{
    public class GameContextBattleState : ICoroutineState
    {
        private static float CHARACTERS_HORIZONTAL_OFFSET = 3f;

        private GameObject playerObject;
        private Camera playerCamera;
        private PlayerInput playerInput;
        private bool goToMap = false;

        private GameObject[] enemyObjects;
        private int deathCount;

        #region ICoroutineState Implementation
        public IEnumerator EnterState()
        {
            deathCount = 0;
            playerInput.SwitchCurrentActionMap("PlayerBattle");
            ObjectActivationAndDesactivation(true);
            ConfigureEnemyObjects();
            // Move camera
            // Move characters
            MoveCharacters();
            Debug.Log("Entering battle state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            ObjectActivationAndDesactivation(false);
            // Yield end battle screen
            // Dispose enemies
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
        }

        public GameContextBattleState(GameObject playerObject, Camera playerCamera, PlayerInput playerInput)
        {
            this.playerObject = playerObject;
            this.playerCamera = playerCamera;
            this.playerInput = playerInput;
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
            return (this.playerObject.transform.position + this.enemyObjects[0].transform.position) / 2f;
        }

        private Vector3 GetCenter(Vector3 originalPosition)
        {
            originalPosition.x = GetCenter().x;
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