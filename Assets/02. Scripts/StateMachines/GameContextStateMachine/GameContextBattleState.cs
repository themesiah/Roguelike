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
        public BattleSystem battleSystem { get; private set; }
        private GameObject[] enemyObjects;
        private int deathCount;
        private RewardData rewardData = null;
        private bool paused = false;
        private ScriptableIntReference battlePositionIntReference;
        private StringGameEvent virtualCameraChangeEvent;
        private RuntimeSingleCinemachineTargetGroup targetGroupRef;

        #region ICoroutineState Implementation
        public IEnumerator EnterState()
        {
            deathCount = 0;
            actionMapSwitchEvent.Raise("PlayerBattle");
            virtualCameraChangeEvent.Raise("BattleCamera");
            ConfigureEnemyObjects();
            SpawnPlayerCompanions();
            // Move characters
            var battlePositionData = BattlePosition.MoveCharacters(this.playerObject, this.enemyObjects, centerCheckLayerMask);
            int position = 0;
            if (targetGroupRef.Get().FindMember(playerObject.transform) < 0)
            {
                targetGroupRef.Get().AddMember(playerObject.transform, 1f, GameConstantsBehaviour.Instance.battleGroupRadius.GetValue());
            }
            foreach(var enemy in enemyObjects)
            {
                targetGroupRef.Get().AddMember(enemy.transform, 1f, GameConstantsBehaviour.Instance.battleGroupRadius.GetValue());
            }
            if (!battlePositionData.direction)
            {
                position = 1;
            }
            foreach (GameObject enemy in enemyObjects)
            {
                if (enemy == enemyObjects[0]) continue;
                enemy.transform.SetParent(enemyObjects[0].transform.parent);
            }
            battlePositionIntReference.SetValue(position);
            // Move Camera
            // TODO: Change virtual camera
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
            // Remove enemies from virtual cam group
            foreach(var enemy in enemyObjects)
            {
                if (enemy != null && targetGroupRef.Get().FindMember(enemy.transform) != -1)
                {
                    targetGroupRef.Get().RemoveMember(enemy.transform);
                }
            }
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
                    rewardData = new RewardData(0, 0, null, null, null, null, null);
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
            ScriptableIntReference hardCurrencyReference, int centerCheckLayerMask, RewardUILibrary rewardUILibrary, ScriptableIntReference battlePositionIntReference,
            StringGameEvent virtualCameraChangeEvent, RuntimeSingleCinemachineTargetGroup targetGroupRef)
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
            this.battleSystem.OnEnemyRemoved += EnemyFlee;
            this.virtualCameraChangeEvent = virtualCameraChangeEvent;
            this.targetGroupRef = targetGroupRef;
        }

        public void PerformTimeStop(bool activate)
        {
            battleSystem.PerformTimeStop(activate);
        }
        #endregion

        #region Private methods
        private void ConfigureEnemyObjects()
        {
            foreach (var go in this.enemyObjects)
            {
                go.GetComponent<CharacterBattleBehaviour>().StatusManager.health.OnDeath += EnemyDeath;
                go.GetComponent<CharacterBattleBehaviour>().StatusManager.health.OnDeath += (h)=> { EnemyDeathCam(go); };
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

        private void EnemyDeathCam(GameObject enemyObject)
        {
            if (targetGroupRef.Get().FindMember(enemyObject.transform) != -1)
            {
                targetGroupRef.Get().RemoveMember(enemyObject.transform);
            }
        }

        private void EnemyFlee(BattleSystem battleSystem, CharacterBattleManager enemy)
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