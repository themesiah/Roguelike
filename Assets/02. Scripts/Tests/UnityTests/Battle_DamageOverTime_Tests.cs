using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Laresistance.Tests {
    public class Battle_DamageOverTime_Tests
    {
        private BattleAbility GetAbilityByIndex(int i)
        {
            float cooldown = 0f;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch(i)
            {
                case 0:
                    effects.Add(new BattleEffectDamageOverTime(3));
                    cooldown = 1f;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cooldown);
            ability.Tick(1.1f);
            return ability;
        }

        private BattleStatusManager GetStatusManager(int health)
        {
            return new BattleStatusManager(new CharacterHealth(health));
        }

        [UnityTest]
        public IEnumerator When_ApplyingSingleDot()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { enemy }, 1);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.1f);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.5f);
            Assert.AreEqual(97, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.5f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_ApplyingMultipleDotsSimultaneous()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { enemy }, 1);
            ability.Tick(1.1f);
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { enemy }, 1);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.1f);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.5f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.5f);
            Assert.AreEqual(88, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_ApplyingMultipleDotsNotSimultaneous()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { enemy }, 1);
            ability.Tick(1.1f);
            enemy.ProcessStatus(0.3f);
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { enemy }, 1);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.3f);
            Assert.AreEqual(97, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.3f);
            Assert.AreEqual(94, enemy.health.GetCurrentHealth());
            enemy.ProcessStatus(0.3f);
            Assert.AreEqual(91, enemy.health.GetCurrentHealth());
            yield return null;
        }
    }
}