using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Systems;
using Laresistance.Battle;
using Laresistance.Core;
using Laresistance.Data;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Laresistance.Behaviours;
using UnityEngine.TestTools;

namespace Laresistance.Tests
{
    public class Player_Equipment_Events_Test3
    {
        private static string EQUIP_PATH = "Assets/63. Game Data/Equipments/";
        private static string ENEMY_PATH = "Assets/63. Game Data/Enemies/";
        private static string MINIONS_PATH = "Assets/63. Game Data/Minions/";
        // 
        private Equipment SetEquipment(Player p, string name)
        {
            EquipmentData data = AssetDatabase.LoadAssetAtPath(EQUIP_PATH + name + ".asset", typeof(EquipmentData)) as EquipmentData;
            Equipment e = EquipmentFactory.GetEquipment(data, p.GetEquipmentEvents(), p.statusManager);
            p.EquipEquipment(e);
            return e;
        }

        private BattleSystem GetBattleSystem(string[] minions, string[] equipments, ScriptableIntReference bloodRef)
        {
            BattleSystem battleSystem = new BattleSystem();

            CharacterBattleManager player = GetPlayer(minions, equipments, bloodRef);
            CharacterBattleManager enemy = GetEnemy("ENEMY_001");

            battleSystem.InitBattle(player, new CharacterBattleManager[] { enemy });

            return battleSystem;
        }

        private CharacterBattleManager GetEnemy(string name)
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

        private CharacterBattleManager GetPlayer(string[] minions, string[] equipments, ScriptableIntReference bloodRef)
        {
            BattleStatusManager playerStatus = new BattleStatusManager(new CharacterHealth(100));
            Player player = new Player(playerStatus);
            playerStatus.SetEquipmentEvents(player.GetEquipmentEvents());

            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(15, EffectTargetType.Enemy, playerStatus));
            BattleAbility testAbility = new BattleAbility(testEffects, 3, 0, 1f, null, player.GetEquipmentEvents());
            player.SetMainAbilities(new BattleAbility[] { testAbility });

            foreach (var mn in minions)
            {
                MinionData md = AssetDatabase.LoadAssetAtPath(MINIONS_PATH + mn + ".asset", typeof(MinionData)) as MinionData;
                Minion m = MinionFactory.GetMinion(md, 1, player.GetEquipmentEvents(), playerStatus);
                player.EquipMinion(m);
            }

            foreach (var en in equipments)
            {
                SetEquipment(player, en);
            }

            IAbilityInputProcessor abilityInput = new EnemyAbilityManager(player.GetAbilities(), 1, new DummyBattleAnimator());
            IAbilityExecutor abilityExecutor = new PlayerAbilityExecutor(player, new DummyBattleAnimator(), bloodRef);

            CharacterBattleManager playerBM = new CharacterBattleManager(playerStatus, abilityInput, abilityExecutor, new DummyTargetSelection(), new DummyBattleAnimator());
            return playerBM;
        }

        [UnityTest]
        public IEnumerator When_No_Equip002()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] {}, new string[] {}, null);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(0.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth, currentHealth);
            yield return battleSystem.Tick(2.5f);
            currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth, currentHealth);
        }

        [UnityTest]
        public IEnumerator When_Equip002()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] {}, new string[] { "EQUIPMENT_002" }, null);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(0.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth, currentHealth);
            yield return battleSystem.Tick(2.5f);
            currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth-12, currentHealth);
        }

        [UnityTest]
        public IEnumerator When_Equip004()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] {}, new string[] { "EQUIPMENT_004" }, null);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(0.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth-15, currentHealth);
        }

        [UnityTest]
        public IEnumerator When_Equip006()
        {
            ScriptableIntReference bloodRef = new ScriptableIntReference();
            bloodRef.SetScriptableValueManually((ScriptableIntValue)ScriptableObject.CreateInstance(typeof(ScriptableIntValue)));
            bloodRef.SetValue(1000);
            Assert.AreEqual(1000, bloodRef.GetValue());

            BattleSystem battleSystem = GetBattleSystem(new string[] { }, new string[] { "EQUIPMENT_006" }, bloodRef);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(3.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth - 18, currentHealth);
            Assert.AreEqual(970, bloodRef.GetValue());
        }

        [UnityTest]
        public IEnumerator When_Equip008()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] { "Minion_003" }, new string[] { "EQUIPMENT_008" }, null);
            int playerStartingHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(75, playerStartingHealth);
            yield return battleSystem.Tick(7.1f);
            yield return battleSystem.Tick(0.1f);
            int currentHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(playerStartingHealth - 1, currentHealth);
        }

        [UnityTest]
        public IEnumerator When_Equip014()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] {  }, new string[] { "EQUIPMENT_014" }, null);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            int playerStartingHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(7.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            int playerCurrentHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth - 20, currentHealth);
            Assert.AreEqual(playerStartingHealth - 9, playerCurrentHealth);
        }

        [UnityTest]
        public IEnumerator When_Equip016()
        {
            BattleSystem battleSystem = GetBattleSystem(new string[] {  }, new string[] { "EQUIPMENT_016" }, null);
            int startingHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            int playerStartingHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            yield return battleSystem.Tick(7.1f);
            int currentHealth = battleSystem.GetSelected().StatusManager.health.GetCurrentHealth();
            int playerCurrentHealth = battleSystem.GetPlayer().StatusManager.health.GetCurrentHealth();
            Assert.AreEqual(startingHealth - 10, currentHealth);
            Assert.AreEqual(playerStartingHealth - 5, playerCurrentHealth);
        }
    }
}