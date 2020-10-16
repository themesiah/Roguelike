using Laresistance.Core;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Laresistance.Systems
{
    public class BattleSystem
    {
        private CharacterBattleManager playerBattleManager;
        private CharacterBattleManager[] enemiesBattleManager;

        public BattleSystem()
        {
            
        }

        public void InitBattle(CharacterBattleManager player, CharacterBattleManager[] enemies)
        {
            this.playerBattleManager = player;
            this.enemiesBattleManager = enemies;

            playerBattleManager.SetAllies(new CharacterBattleManager[] { player });
            playerBattleManager.SetEnemies(enemies);
            playerBattleManager.StartBattle();
            foreach(var enemy in enemiesBattleManager)
            {
                enemy.SetEnemies(new CharacterBattleManager[] { player });
                enemy.SetAllies(enemies);
                enemy.StartBattle();
            }
        }

        public void EndBattle()
        {
            playerBattleManager.EndBattle();
            foreach (var enemy in enemiesBattleManager)
            {
                enemy.EndBattle();
            }

            playerBattleManager = null;
            enemiesBattleManager = null;
        }

        private float lastTime = 0f;
        public IEnumerator Tick(float delta)
        {
            float customDelta = delta;
            if (lastTime != 0)
            {
                customDelta = Time.time - lastTime;
            }
            lastTime = Time.time;
            int playerAbilityIndex = playerBattleManager.Tick(customDelta);
            if (playerAbilityIndex != -1)
            {
                yield return playerBattleManager.ExecuteSkill(playerAbilityIndex);
            }
            foreach(var bm in enemiesBattleManager)
            {
                int enemyAbilityIndex = bm.Tick(customDelta);
                if (enemyAbilityIndex != -1)
                {
                    yield return bm.ExecuteSkill(enemyAbilityIndex);
                }
            }
        }
    }
}