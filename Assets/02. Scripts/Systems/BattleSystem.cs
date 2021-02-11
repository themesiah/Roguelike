using GamedevsToolbox.Utils;
using Laresistance.Battle;
using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public delegate void OnTimeStopActivationHandler(BattleSystem sender, bool activate);
        public event OnTimeStopActivationHandler OnTimeStopActivation;
        public delegate void OnTimeStopDeltaModifierHandler(BattleSystem sender, float modifier);
        public event OnTimeStopDeltaModifierHandler OnTimeStopDeltaModifier;
        public delegate void OnEnemyRemovedHandler(BattleSystem sender, CharacterBattleManager enemy);
        public event OnEnemyRemovedHandler OnEnemyRemoved;

        public BattleSystem()
        {
            selectedEnemy = null;
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
            BattleAbilityManager.StartBattle();
            OnTimeStopDeltaModifier?.Invoke(this, 0f);
        }

        public void EndBattle()
        {
            UnityEngine.Debug.Log("End battle!");
            PerformTimeStop(false);
            BattleAbilityManager.StopBattle();
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