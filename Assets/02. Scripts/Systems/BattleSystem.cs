using GamedevsToolbox.Utils;
using Laresistance.Battle;
using Laresistance.Core;
using System.Collections;
using System.Collections.Generic;

namespace Laresistance.Systems
{
    public class BattleSystem : IPausable
    {
        private CharacterBattleManager playerBattleManager;
        private List<CharacterBattleManager> enemiesBattleManager;

        private CharacterBattleManager selectedEnemy;
        private bool paused = false;
        private bool battling = false;

        public BattleSystem()
        {
            selectedEnemy = null;
        }

        public void InitBattle(CharacterBattleManager player, CharacterBattleManager[] enemies)
        {
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
                };
            }
            SelectNext();
            battling = true;
            BattleAbilityManager.StartBattle();
        }

        public void EndBattle()
        {
            UnityEngine.Debug.Log("End battle!");
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
            Unselect();
            selectedEnemy = enemiesBattleManager[index];
            if (!enemiesBattleManager[index].Select())
            {
                SelectNext();
            }
        }

        private void Unselect()
        {
            selectedEnemy?.Unselect();
            selectedEnemy = null;
        }

        public CharacterBattleManager GetSelected()
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
            enemiesBattleManager[indexToRemove].EndBattle();
            enemiesBattleManager.RemoveAt(indexToRemove);
        }

        public IEnumerator Tick(float delta)
        {
            if (!paused && battling)
            {
                int playerAbilityIndex = playerBattleManager.Tick(delta);
                if (playerAbilityIndex != -1)
                {
                    yield return playerBattleManager.ExecuteSkill(playerAbilityIndex);
                }
                foreach (var bm in enemiesBattleManager)
                {
                    int enemyAbilityIndex = bm.Tick(delta);
                    if (enemyAbilityIndex != -1)
                    {
                        yield return bm.ExecuteSkill(enemyAbilityIndex);
                    }
                }
            }
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