using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.Utils;
using Laresistance.Battle;
using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Laresistance.Systems
{
    public class BattleSystem : IPausable, ITimeStoppable, ITargetDependant
    {
        private CharacterBattleManager playerBattleManager;
        private List<CharacterBattleManager> enemiesBattleManager;

        private CharacterBattleManager selectedEnemy;
        private bool paused = false;
        private bool battling = false;

        private static float BATTLE_ENERGY_SPEED_ACCELERATION = 0.00f;
        private static float MAX_BATTLE_ENERGY_SPEED_MODIFIER = 4f;
        private float currentEnergySpeedModifier;
        private bool stoppedTime = false;

        private float stopTimeDelayTimer = 0f;

        private static int battlesFinished = 0;

        public delegate void OnTimeStopActivationHandler(BattleSystem sender, bool activate);
        public event OnTimeStopActivationHandler OnTimeStopActivation;
        public delegate void OnTimeStopDeltaModifierHandler(BattleSystem sender, float modifier);
        public event OnTimeStopDeltaModifierHandler OnTimeStopDeltaModifier;
        public delegate void OnEnemyRemovedHandler(BattleSystem sender, CharacterBattleManager enemy);
        public event OnEnemyRemovedHandler OnEnemyRemoved;

        // References
        private ScriptableIntReference difficultyRef;
        private int characterIndex;
        private CharacterBattleManager[] enemiesTemp;
        private CharacterBattleManager playerTemp;
        private Player playerRef;

        public BattleSystem(ScriptableIntReference difficultyRef = null, int characterIndex = -1, Player playerRef = null)
        {
            selectedEnemy = null;
            this.difficultyRef = difficultyRef;
            this.characterIndex = characterIndex;
            this.playerRef = playerRef;
        }

        public void InitBattle(CharacterBattleManager player, CharacterBattleManager[] enemies)
        {
            currentEnergySpeedModifier = 1f;
            this.playerBattleManager = player;
            this.enemiesBattleManager = new List<CharacterBattleManager>(enemies);

            playerBattleManager.SetBattleSystem(this);

            playerBattleManager.SetAllies(new CharacterBattleManager[] { player });
            playerBattleManager.SetEnemies(enemies);
            playerBattleManager.StartBattle();
            playerBattleManager.StatusManager.health.OnDeath += (CharacterHealth h) =>
            {
                playerBattleManager.Die();
            };
            foreach(var enemy in enemiesBattleManager)
            {
                enemy.SetBattleSystem(this);
                enemy.SetEnemies(new CharacterBattleManager[] { player });
                enemy.SetAllies(enemies);
                enemy.StartBattle();
                enemy.StatusManager.health.OnDeath += (CharacterHealth h) => {
                    enemy.Die();
                    enemiesBattleManager.Remove(enemy);
                };
            }
            SelectNext();
            battling = true;
            BattleAbilityManager.Instance.StartBattle();
            OnTimeStopDeltaModifier?.Invoke(this, 0f);

            enemiesTemp = enemies;
            playerTemp = player;
        }

        public void EndBattle()
        {
            UnityEngine.Debug.Log("End battle!");
            PerformTimeStop(false);
            BattleAbilityManager.Instance.StopBattle();
            Unselect();
            playerBattleManager.EndBattle();
            foreach (var enemy in enemiesBattleManager)
            {
                enemy.EndBattle();
            }

            playerBattleManager = null;
            enemiesBattleManager = null;
            battling = false;
            OnTimeStopDeltaModifier?.Invoke(this, 0f);
            SendEndBattleEvent();
        }

        private void SendEndBattleEvent()
        {
            // Battles finished
            BattleSystem.battlesFinished++;
            // Character
            string playerCharacter = GameConstantsBehaviour.Instance.characterName[characterIndex];
            // Player minions
            string minion1name = "";
            int minion1Level = 0;
            string minion2name = "";
            int minion2Level = 0;
            string minion3name = "";
            int minion3Level = 0;
            if (playerRef.EquippedMinionsQuantity >= 1)
            {
                minion1name = playerRef.GetMinions()[0].Data.name;
                minion1Level = playerRef.GetMinions()[0].Level;
            }
            if (playerRef.EquippedMinionsQuantity >= 2)
            {
                minion2name = playerRef.GetMinions()[1].Data.name;
                minion2Level = playerRef.GetMinions()[1].Level;
            }
            if (playerRef.EquippedMinionsQuantity >= 3)
            {
                minion3name = playerRef.GetMinions()[2].Data.name;
                minion3Level = playerRef.GetMinions()[2].Level;
            }
            // Player equipments
            string equipment1Name = "";
            string equipment2Name = "";
            string equipment3Name = "";
            string equipment4Name = "";
            if (playerRef.GetEquipments().Length >= 1 && playerRef.GetEquipments()[0] != null)
            {
                equipment1Name = playerRef.GetEquipments()[0].Data.name;
            }
            if (playerRef.GetEquipments().Length >= 2 && playerRef.GetEquipments()[1] != null)
            {
                equipment2Name = playerRef.GetEquipments()[1].Data.name;
            }
            if (playerRef.GetEquipments().Length >= 3 && playerRef.GetEquipments()[2] != null)
            {
                equipment3Name = playerRef.GetEquipments()[2].Data.name;
            }
            if (playerRef.GetEquipments().Length >= 4 && playerRef.GetEquipments()[3] != null)
            {
                equipment4Name = playerRef.GetEquipments()[3].Data.name;
            }
            // Enemy data
            string enemy1name = "";
            string enemy2name = "";
            string enemy3name = "";
            int enemy1level = 0;
            int enemy2level = 0;
            int enemy3level = 0;
            if (enemiesTemp.Length >= 1)
            {
                enemy1name = ((EnemyBattleBehaviour)enemiesTemp[0].StatusManager.ParentBattleBehaviour).GetEnemyData().name;
                enemy1level = ((EnemyBattleBehaviour)enemiesTemp[0].StatusManager.ParentBattleBehaviour).EnemyLevel;
            }
            if (enemiesTemp.Length >= 2)
            {
                enemy2name = ((EnemyBattleBehaviour)enemiesTemp[1].StatusManager.ParentBattleBehaviour).GetEnemyData().name;
                enemy2level = ((EnemyBattleBehaviour)enemiesTemp[1].StatusManager.ParentBattleBehaviour).EnemyLevel;
            }
            if (enemiesTemp.Length >= 3)
            {
                enemy3name = ((EnemyBattleBehaviour)enemiesTemp[2].StatusManager.ParentBattleBehaviour).GetEnemyData().name;
                enemy3level = ((EnemyBattleBehaviour)enemiesTemp[2].StatusManager.ParentBattleBehaviour).EnemyLevel;
            }

            AnalyticsSystem.Instance.CustomEvent("BattleEnd", new Dictionary<string, object>() {
                { "Win", true },
                { "BattlesFinished", BattleSystem.battlesFinished },
                { "Difficulty", difficultyRef.GetValue() },
                { "Character", playerCharacter },
                { "Minion1Name", minion1name },
                { "Minion2Name", minion2name },
                { "Minion3Name", minion3name },
                { "Minion1Level", minion1Level },
                { "Minion2Level", minion2Level },
                { "Minion3Level", minion3Level },
                { "Equip1", equipment1Name },
                { "Equip2", equipment2Name },
                { "Equip3", equipment3Name },
                { "Equip4", equipment4Name },
                { "Enemy1Name", enemy1name },
                { "Enemy2Name", enemy2name },
                { "Enemy3Name", enemy3name },
                { "Enemy1Level", enemy1level },
                { "Enemy2Level", enemy2level },
                { "Enemy3Level", enemy3level }
            });
        }

        public void SelectNext()
        {
            if (selectedEnemy == null)
            {
                Select(0);
            }
            else
            {
                for (int i = 0; i < enemiesBattleManager.Count; ++i)
                {
                    if (selectedEnemy == enemiesBattleManager[i])
                    {
                        if (i == enemiesBattleManager.Count - 1)
                        {
                            Select(0);
                        } else
                        {
                            Select(i + 1);
                        }
                        break;
                    }
                }
            }
        }

        public void SelectPrevious()
        {
            if (selectedEnemy == null)
            {
                Select(0);
            }
            else
            {
                for (int i = 0; i < enemiesBattleManager.Count; ++i)
                {
                    if (selectedEnemy == enemiesBattleManager[i])
                    {
                        if (i == 0)
                        {
                            Select(enemiesBattleManager.Count - 1);
                        }
                        else
                        {
                            Select(i - 1);
                        }
                        break;
                    }
                }
            }
        }

        public void Reselect()
        {
            if (selectedEnemy == null)
            {
                Select(0);
            }
            else
            {
                for (int i = 0; i < enemiesBattleManager.Count; ++i)
                {
                    if (selectedEnemy == enemiesBattleManager[i])
                    {
                        Select(i);
                        break;
                    }
                }
            }
        }

        private void Select(int index)
        {
            if (AllEnemiesDead())
                return;
            SetSelectedTarget(enemiesBattleManager[index]);
        }

        public void SetSelectedTarget(CharacterBattleManager cbm)
        {
            Unselect();
            selectedEnemy = cbm;
            if (!selectedEnemy.Select())
            {
                SelectNext();
            } else
            {
                playerBattleManager.SetSelectedTarget(cbm);
            }
        }

        private void Unselect()
        {
            selectedEnemy?.Unselect();
            selectedEnemy = null;
        }

        public CharacterBattleManager GetSelectedTarget()
        {
            return selectedEnemy;
        }

        public CharacterBattleManager GetPlayer()
        {
            return playerBattleManager;
        }

        public CharacterBattleManager[] GetEnemies()
        {
            if (enemiesBattleManager == null)
                return null;
            else
                return enemiesBattleManager.ToArray();
        }

        public bool AllEnemiesDead()
        {
            foreach(var enemy in enemiesBattleManager)
            {
                if (!enemy.dead)
                    return false;
            }
            return true;
        }

        public bool PlayerDead()
        {
            return playerBattleManager.dead;
        }

        public void RemoveEnemyAt(int indexToRemove)
        {
            UnityEngine.Assertions.Assert.IsTrue(indexToRemove >= 0);
            UnityEngine.Assertions.Assert.IsTrue(indexToRemove < enemiesBattleManager.Count);
            if (selectedEnemy == enemiesBattleManager[indexToRemove])
            {
                SelectNext();
            }
            CharacterBattleManager enemy = enemiesBattleManager[indexToRemove];
            enemy.EndBattle();
            enemiesBattleManager.Remove(enemy);
            //if (AllEnemiesDead())
            //{
            //    EndBattle();
            //}
            OnEnemyRemoved?.Invoke(this, enemy);
        }

        public IEnumerator Tick(float delta)
        {
            if (!paused && battling)
            {
                float finalDelta = delta;
                float deltaModifier = 1f;

                // Modify delta time
                if (stopTimeDelayTimer > 0f)
                {
                    stopTimeDelayTimer -= delta;
                    stopTimeDelayTimer = Mathf.Max(stopTimeDelayTimer, 0f);
                }
                deltaModifier = stopTimeDelayTimer / GameConstantsBehaviour.Instance.stopTimeDelay.GetValue();
                if (!stoppedTime)
                {
                    deltaModifier = 1f - deltaModifier;
                }
                finalDelta *= deltaModifier;
                OnTimeStopDeltaModifier?.Invoke(this, 1f - deltaModifier);

                // Player battle manager always tick, with different delta depending on time stopped or not
                AbilityExecutionData playerAbilityExecutionData = playerBattleManager.Tick(finalDelta, currentEnergySpeedModifier);
                
                if(playerAbilityExecutionData.index != -1)
                {
                    yield return playerBattleManager.ExecuteSkill(playerAbilityExecutionData.index,playerAbilityExecutionData.selectedTarget);
                }
                for (int i = enemiesBattleManager.Count-1; i >= 0; --i)
                {
                    var bm = enemiesBattleManager[i];
                    AbilityExecutionData enemyAbilityExecutionData = bm.Tick(finalDelta, currentEnergySpeedModifier);
                    if (enemyAbilityExecutionData.index != -1)
                    {
                        yield return bm.ExecuteSkill(enemyAbilityExecutionData.index, enemyAbilityExecutionData.selectedTarget);
                    }
                }
                currentEnergySpeedModifier = Mathf.Min(MAX_BATTLE_ENERGY_SPEED_MODIFIER, currentEnergySpeedModifier + BATTLE_ENERGY_SPEED_ACCELERATION * finalDelta);
            }
        }

        public void PerformTimeStop(bool activate)
        {
            if (stoppedTime != activate && playerBattleManager != null)
            {
                OnTimeStopActivation?.Invoke(this, activate);
                stopTimeDelayTimer = GameConstantsBehaviour.Instance.stopTimeDelay.GetValue();

                playerBattleManager.PerformTimeStop(activate);
                foreach (var bm in enemiesBattleManager)
                {
                    bm.PerformTimeStop(activate);
                }
            }
            stoppedTime = activate;
        }

        public void Pause()
        {
            paused = true;
        }

        public void Resume()
        {
            paused = false;
        }
    }
}