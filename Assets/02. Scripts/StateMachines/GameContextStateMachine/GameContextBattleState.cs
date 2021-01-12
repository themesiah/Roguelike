using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Battle;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.StateMachines
{
    public class GameContextBattleState : ICoroutineState
    {
        private GameObject playerObject;
        private StringGameEvent actionMapSwitchEvent;
        private int centerCheckLayerMask;
        private bool goToMap = false;
        private bool forcedStop = false;
        private bool removeFirstEnemy = false;
        private RewardSystem rewardSystem;
        private PlayerMinionCompanionSpawner companionSpawner;
        private BattleSystem battleSystem;
        private GameObject[] enemyObjects;
        private int deathCount;
        private RewardData rewardData = null;
        private bool paused = false;
        private ScriptableIntReference battlePositionIntReference;

        #region ICoroutineState Implementation
        public IEnumerator EnterState()
        {
            deathCount = 0;
            actionMapSwitchEvent.Raise("PlayerBattle");
            ConfigureEnemyObjects();
            SpawnPlayerCompanions();
            // Move characters
            bool right = BattlePosition.MoveCharacters(this.playerObject, this.enemyObjects, centerCheckLayerMask);
            int position = 0;
            if (!right)
            {
                position = 1;
            }
            battlePositionIntReference.SetValue(position);
            // Init battle system
            CharacterBattleManager[] enemies = new CharacterBattleManager[enemyObjects.Length];
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i] = enemyObjects[i].GetComponent<CharacterBattleBehaviour>().battleManager;
            }
            battleSystem.InitBattle(playerObject.GetComponent<PlayerBattleBehaviour>().battleManager, enemies);            
            GamedevsToolbox.Utils.Logger.Logger.Log("Entering battle state");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            // Deactivate things
            companionSpawner.Despawn();
            battleSystem.EndBattle();
            // Yield end battle screen and give rewards
            if (!forcedStop)
            {
                actionMapSwitchEvent.Raise("UI");
                yield return rewardSystem.GetReward(rewardData);
            }
            goToMap = false;
            forcedStop = false;
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
            } else if (signal == "RemoveFirstEnemy")
            {
                removeFirstEnemy = true;
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
                } else if (removeFirstEnemy)
                {
                    removeFirstEnemy = false;
                    battleSystem.RemoveEnemyAt(0);
                    rewardData = new RewardData(0, 0, null, null, null, null);
                    deathCount++;
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

        public GameContextBattleState(GameObject playerObject, Camera playerCamera, StringGameEvent actionMapSwitchEvent, ScriptableIntReference bloodReference,
            ScriptableIntReference hardCurrencyReference, int centerCheckLayerMask, RewardUILibrary rewardUILibrary, ScriptableIntReference battlePositionIntReference)
        {
            this.playerObject = playerObject;
            this.actionMapSwitchEvent = actionMapSwitchEvent;
            this.centerCheckLayerMask = centerCheckLayerMask;
            Player player = playerObject.GetComponent<PlayerDataBehaviour>().player;
            this.rewardSystem = new RewardSystem(player, bloodReference, hardCurrencyReference, rewardUILibrary);
            this.companionSpawner = new PlayerMinionCompanionSpawner(player);
            this.battleSystem = new BattleSystem();
            this.battlePositionIntReference = battlePositionIntReference;
            playerObject.GetComponent<CharacterBattleBehaviour>().StatusManager.health.OnDeath += PlayerDeath;
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
            companionSpawner.Spawn(playerObject.transform.position.x < enemyObjects[0].transform.position.x, playerObject);
        }

        private void EnemyDeath(CharacterHealth health)
        {
            deathCount++;
            if (deathCount == enemyObjects.Length)
            {
                goToMap = true;
            }
        }

        private void PlayerDeath(CharacterHealth health)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenario2");
        }
        #endregion
    }
}