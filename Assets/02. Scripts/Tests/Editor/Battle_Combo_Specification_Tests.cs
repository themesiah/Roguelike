using NUnit.Framework;
using Laresistance.Core;
using Laresistance.Battle;
using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using Laresistance.Data;
using UnityEngine;
using Laresistance.Behaviours;
using UnityEditor;

namespace Laresistance.Tests
{
    public class Battle_Combo_Specification_Tests : MonoBehaviour
    {
        private static string ABILITIES_PATH = "Assets/63. Game Data/BattleAbilities/";
        private static string ATTACK_ABILITY_LOW_PATH = ABILITIES_PATH + "ABILITY_ATTACK_LOW.asset";
        private static string ATTACK_ABILITY_MID_PATH = ABILITIES_PATH + "ABILITY_ATTACK_MID.asset";
        private static string POISON_ABILITY_LOW_PATH = ABILITIES_PATH + "ABILITY_DOT_LOW.asset";

        private static string COMBOS_PATH = "Assets/63. Game Data/Combos/";
        private static string COMBO1_PATH = COMBOS_PATH + "Combo1/Combo1.asset";

        private BattleStatusManager GetBattleStatus()
        {
            return new BattleStatusManager(null, new CharacterHealth(100));
        }

        private AbilityData GetAbilityData(string path)
        {
            AbilityData ad = AssetDatabase.LoadAssetAtPath(path, typeof(AbilityData)) as AbilityData;
            return ad;
        }

        private ComboData GetComboData(string path)
        {
            ComboData cd = AssetDatabase.LoadAssetAtPath(path, typeof(ComboData)) as ComboData;
            return cd;
        }

        [Test]
        public void When_CheckingCorrectComboWithSameLength()
        {
            BattleStatusManager status = GetBattleStatus();
            AbilityData ad1 = GetAbilityData(ATTACK_ABILITY_LOW_PATH);
            AbilityData ad2 = GetAbilityData(ATTACK_ABILITY_MID_PATH);
            BattleAbility ability1 = BattleAbilityFactory.GetBattleAbility(ad1, null, status);
            BattleAbility ability2 = BattleAbilityFactory.GetBattleAbility(ad2, null, status);
            ComboData comboData = GetComboData(COMBO1_PATH);
            ComboCondition comboCondition = new ComboCondition(comboData.ComboCondition);
            
            bool satisfies = comboCondition.IsSatisfiedBy(new BattleAbility[] { ability1, ability2 });
            Assert.IsTrue(satisfies);
        }

        [Test]
        public void When_CheckingIncorrectComboWithSameLength()
        {
            BattleStatusManager status = GetBattleStatus();
            AbilityData ad1 = GetAbilityData(ATTACK_ABILITY_LOW_PATH);
            AbilityData ad2 = GetAbilityData(POISON_ABILITY_LOW_PATH);
            BattleAbility ability1 = BattleAbilityFactory.GetBattleAbility(ad1, null, status);
            BattleAbility ability2 = BattleAbilityFactory.GetBattleAbility(ad2, null, status);
            ComboData comboData = GetComboData(COMBO1_PATH);
            ComboCondition comboCondition = new ComboCondition(comboData.ComboCondition);

            bool satisfies = comboCondition.IsSatisfiedBy(new BattleAbility[] { ability1, ability2 });
            Assert.IsFalse(satisfies);
        }

        [Test]
        public void When_CheckingCorrectComboWithDifferentLengthOnStart()
        {
            BattleStatusManager status = GetBattleStatus();
            AbilityData ad1 = GetAbilityData(ATTACK_ABILITY_LOW_PATH);
            AbilityData ad2 = GetAbilityData(ATTACK_ABILITY_MID_PATH);
            AbilityData ad3 = GetAbilityData(POISON_ABILITY_LOW_PATH);
            BattleAbility ability1 = BattleAbilityFactory.GetBattleAbility(ad1, null, status);
            BattleAbility ability2 = BattleAbilityFactory.GetBattleAbility(ad2, null, status);
            BattleAbility ability3 = BattleAbilityFactory.GetBattleAbility(ad3, null, status);
            ComboData comboData = GetComboData(COMBO1_PATH);
            ComboCondition comboCondition = new ComboCondition(comboData.ComboCondition);

            bool satisfies = comboCondition.IsSatisfiedBy(new BattleAbility[] { ability1, ability2, ability3 });
            Assert.IsTrue(satisfies);
        }

        [Test]
        public void When_CheckingCorrectComboWithDifferentLengthNotOnStart()
        {
            BattleStatusManager status = GetBattleStatus();
            AbilityData ad1 = GetAbilityData(ATTACK_ABILITY_LOW_PATH);
            AbilityData ad2 = GetAbilityData(ATTACK_ABILITY_MID_PATH);
            AbilityData ad3 = GetAbilityData(POISON_ABILITY_LOW_PATH);
            BattleAbility ability1 = BattleAbilityFactory.GetBattleAbility(ad1, null, status);
            BattleAbility ability2 = BattleAbilityFactory.GetBattleAbility(ad2, null, status);
            BattleAbility ability3 = BattleAbilityFactory.GetBattleAbility(ad3, null, status);
            ComboData comboData = GetComboData(COMBO1_PATH);
            ComboCondition comboCondition = new ComboCondition(comboData.ComboCondition);

            bool satisfies = comboCondition.IsSatisfiedBy(new BattleAbility[] { ability3, ability1, ability2  });
            Assert.IsFalse(satisfies);
        }

        [Test]
        public void When_CheckingInorrectComboWithLessLength()
        {
            BattleStatusManager status = GetBattleStatus();
            AbilityData ad1 = GetAbilityData(ATTACK_ABILITY_LOW_PATH);
            BattleAbility ability1 = BattleAbilityFactory.GetBattleAbility(ad1, null, status);
            ComboData comboData = GetComboData(COMBO1_PATH);
            ComboCondition comboCondition = new ComboCondition(comboData.ComboCondition);

            bool satisfies = comboCondition.IsSatisfiedBy(new BattleAbility[] { ability1 });
            Assert.IsFalse(satisfies);
        }
    }
}