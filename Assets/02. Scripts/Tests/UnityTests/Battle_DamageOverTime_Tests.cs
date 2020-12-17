﻿using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Laresistance.Tests {
    public class Battle_DamageOverTime_Tests
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
                    effects.Add(new BattleEffectDamageOverTime(3, Data.EffectTargetType.Enemy, GetStatus()));
                    cost = 1;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cost, 0, 1f, null);
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
        public IEnumerator When_ApplyingSingleDot()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.1f, 1f);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f, 1f);
            Assert.AreEqual(97, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f, 1f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_ApplyingMultipleDotsSimultaneous()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            ability.Tick(1.1f);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.1f, 1f);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f, 1f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f, 1f);
            Assert.AreEqual(88, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_ApplyingMultipleDotsNotSimultaneous()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            ability.Tick(1.1f);
            enemy.ProcessStatus(1.5f/2f, 1f);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f / 2f, 1f);
            Assert.AreEqual(97, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f / 2f, 1f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(1.5f / 2f, 1f);
            Assert.AreEqual(91, enemy.health.GetCurrentHealth());
            yield return null;
        }
    }
}