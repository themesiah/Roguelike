﻿using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Laresistance.Tests {
    public class Battle_SpeedChange_Tests
    {
        private BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        private BattleAbility GetAbilityByIndex(int i)
        {
            int cost = 0;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch(i)
            {
                case 0:
                    effects.Add(new BattleEffectHeal(10, Data.EffectTargetType.Self, GetStatus()));
                    cost = 1;
                    break;
                case 1:
                    effects.Add(new BattleEffectSpeed(50, Data.EffectTargetType.Self, GetStatus()));
                    cost = 1;
                    break;
                case 2:
                    effects.Add(new BattleEffectSpeed(200, Data.EffectTargetType.Self, GetStatus()));
                    cost = 1;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cost, 0, null);
            ability.Tick(1.1f);
            return ability;
        }

        private BattleStatusManager GetStatusManager(int health)
        {
            return new BattleStatusManager(new CharacterHealth(health));
        }

        [OneTimeSetUp]
        public void TestSetup()
        {
            BattleAbilityManager.StartBattle();
        }

        [OneTimeTearDown]
        public void TestTearDown()
        {
            BattleAbilityManager.StopBattle();
        }

        [UnityTest]
        public IEnumerator When_DoingMultipleAbilitiesNoSpeedChange()
        {
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DoingMultipleAbilitiesSlowed()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(1);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsFalse(ability.CanBeUsed());
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DoingMultipleAbilitiesAccelerated()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(2);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            ability.Tick(0.6f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            ability.Tick(0.2f * player.GetSpeedModifier());
            Assert.IsFalse(ability.CanBeUsed());
            yield return null;
        }

        
    }
}