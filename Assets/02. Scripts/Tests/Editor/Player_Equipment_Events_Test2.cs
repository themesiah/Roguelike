using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Battle;
using Laresistance.Core;
using Laresistance.Data;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Laresistance.Tests
{
    public class Player_Equipment_Events_Test2
    {
        private static string PATH = "Assets/63. Game Data/Equipments/";
        // 
        private Equipment GetEquipment(EquipmentEvents ee, BattleStatusManager statusManager, string name)
        {
            Player p = new Player(statusManager);
            EquipmentData data = AssetDatabase.LoadAssetAtPath(PATH + name + ".asset", typeof(EquipmentData)) as EquipmentData;
            Equipment e = EquipmentFactory.GetEquipment(data, ee, statusManager);
            p.EquipEquipment(e);
            return e;
        }

        [Test]
        public void When_Equip002()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_002");
            int power = 10;
            float cooldown = 5f;
            ee.OnGetPower.Invoke(ref power);
            ee.OnGetEnergyProduction.Invoke(ref cooldown);
            Assert.AreEqual(8, power);
            Assert.AreEqual(4f, cooldown);
        }

        [Test]
        public void When_Equip004()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_004");
            float energy = 0f;
            float energyProduction = 5f;
            ee.OnGetStartingEnergy.Invoke(ref energy);
            ee.OnGetEnergyProduction.Invoke(ref energyProduction);
            Assert.AreEqual(1f, energy);
            Assert.AreEqual(6f, energyProduction);
        }

        [Test]
        public void When_Equip006()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_006");
            int power = 10;
            ScriptableIntReference blood = new ScriptableIntReference();
            blood.SetScriptableValueManually(new ScriptableIntValue());
            blood.SetValue(1000);
            ee.OnGetAttackPower.Invoke(ref power);
            ee.OnGetAttackAbilityBloodCost.Invoke(blood);
            Assert.AreEqual(12, power);
            Assert.AreEqual(970, blood.GetValue());
        }

        [Test]
        public void When_Equip008()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_008");
            int power = 10;
            int health = 100;
            ee.OnGetHealPower.Invoke(ref power);
            ee.OnGetMaxHealth.Invoke(ref health);
            Assert.AreEqual(13, power);
            Assert.AreEqual(75, health);
        }

        [Test]
        public void When_Equip010()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_010");
            int extraBlood = 1000;
            int bloodLoss = 0;
            ee.OnGetExtraBlood.Invoke(ref extraBlood);
            ee.OnBloodLossPerSecond.Invoke(ref bloodLoss);
            Assert.AreEqual(1300, extraBlood);
            Assert.AreEqual(1, bloodLoss);
        }

        [Test]
        public void When_Equip012()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_012");
            int upgradePrice = 1000;
            int shopPrice = 1000;
            ee.OnUpgradePrice.Invoke(ref upgradePrice);
            ee.OnShopPrice.Invoke(ref shopPrice);
            Assert.AreEqual(800, upgradePrice);
            Assert.AreEqual(1200, shopPrice);
        }

        [Test]
        public void When_Equip014()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_014");
            int retaliation = 0;
            int damageReceived = 10;
            ee.OnRetaliationDamageFlat.Invoke(ref damageReceived, ref retaliation);
            ee.OnDamageReceivedModifier.Invoke(ref damageReceived);
            Assert.AreEqual(5, retaliation);
            Assert.AreEqual(11, damageReceived);
        }

        [Test]
        public void When_Equip016()
        {
            EquipmentEvents ee = new EquipmentEvents();
            BattleStatusManager statusManager = new BattleStatusManager(new CharacterHealth(100));
            Equipment e = GetEquipment(ee, statusManager, "EQUIPMENT_016");
            int damageReceived = 10;
            int damageCaused = 10;
            ee.OnDamageReceivedModifierFlat.Invoke(ref damageReceived);
            ee.OnGetAttackPowerFlat.Invoke(ref damageCaused);
            Assert.AreEqual(7, damageReceived);
            Assert.AreEqual(5, damageCaused);
        }
    }
}