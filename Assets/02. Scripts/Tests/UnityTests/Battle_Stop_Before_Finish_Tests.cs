using Laresistance.Battle;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Systems;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Laresistance.Tests
{
    public class Battle_Stop_Before_Finish_Tests : MonoBehaviour
    {
        private static string ENEMY_PATH = "Assets/63. Game Data/Enemies/";

        private BattleSystem GetBattleSystem()
        {
            BattleSystem bs = new BattleSystem();
            CharacterBattleManager cbm1 = GetCharacterBattleManager("ENEMY_001");
            CharacterBattleManager cbm2 = GetCharacterBattleManager("ENEMY_002");

            bs.InitBattle(cbm1, new CharacterBattleManager[] { cbm2 });

            return bs;
        }

        private CharacterBattleManager GetCharacterBattleManager(string name)
        {
            EnemyData enemyData = AssetDatabase.LoadAssetAtPath(ENEMY_PATH + name + ".asset", typeof(EnemyData)) as EnemyData;
            BattleStatusManager enemyStatus = new BattleStatusManager(new CharacterHealth(enemyData.MaxHealth));
            BattleAbility[] abilities = new BattleAbility[enemyData.AbilitiesData.Length];
            for (int i = 0; i < enemyData.AbilitiesData.Length; ++i)
            {
                abilities[i] = BattleAbilityFactory.GetBattleAbility(enemyData.AbilitiesData[i], null, enemyStatus);
            }

            IAbilityInputProcessor abilityInput = new EnemyAbilityManager(abilities, 1, new DummyBattleAnimator());

            CharacterBattleManager enemy = new CharacterBattleManager(enemyStatus, abilityInput, (IAbilityExecutor)abilityInput, new DummyTargetSelection(), new DummyBattleAnimator());
            return enemy;
        }

        [UnityTest]
        public IEnumerator When_Stopping_Battle_Just_After_Start()
        {
            BattleSystem battleSystem = GetBattleSystem();
            int startingEnemyHealth = battleSystem.GetEnemies()[0].StatusManager.health.GetCurrentHealth();
            int startingPlayerHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            CharacterBattleManager cbm1 = battleSystem.GetPlayer();
            CharacterBattleManager cbm2 = battleSystem.GetEnemies()[0];
            yield return battleSystem.Tick(7.5f);
            Assert.AreEqual(startingEnemyHealth - 8, battleSystem.GetEnemies()[0].StatusManager.health.GetCurrentHealth());
            battleSystem.EndBattle();
            Assert.IsNull(battleSystem.GetPlayer());
            Assert.IsNull(battleSystem.GetEnemies());
            Assert.AreEqual(startingEnemyHealth - 8, cbm2.StatusManager.health.GetCurrentHealth());
            Assert.AreEqual(startingPlayerHealth, cbm1.StatusManager.health.GetCurrentHealth());
        }
    }
}