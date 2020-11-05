using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Laresistance.Tests {
    public class Battle_Damage_Tests
    {
        private BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        private BattleAbility GetAbilityByIndex(int i)
        {
            float cooldown = 0f;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch(i)
            {
                case 0:
                    effects.Add(new BattleEffectDamage(10, Data.EffectTargetType.Enemy, GetStatus()));
                    cooldown = 1f;
                    break;
                case 1:
                    effects.Add(new BattleEffectHeal(10, Data.EffectTargetType.Self, GetStatus()));
                    cooldown = 1f;
                    break;
                case 2:
                    effects.Add(new BattleEffectShield(10, Data.EffectTargetType.Self, GetStatus()));
                    cooldown = 1f;
                    break;
                case 3:
                    effects.Add(new BattleEffectShield(5, Data.EffectTargetType.Self, GetStatus()));
                    cooldown = 1f;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cooldown, null);
            ability.Tick(1.1f);
            return ability;
        }

        private BattleStatusManager GetStatusManager(int health)
        {
            return new BattleStatusManager(new CharacterHealth(health));
        }

        [UnityTest]
        public IEnumerator When_DealingDamage()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDamageTwiceWithoutWaiting()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDamageTwiceWithWaiting()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            ability.Tick(1.1f);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(80, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDamageAndHealing()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(1);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { enemy }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_HealingMoreThanMax()
        {
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(1);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.AreEqual(100, player.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDamageWithShield()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(2);
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { enemy }, new BattleStatusManager[] { enemy }, 1, null);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(100, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDamageAfterShield()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(2);
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { enemy }, new BattleStatusManager[] { enemy }, 1, null);
            enemy.ProcessStatus(CharacterHealth.SHIELD_DURATION+0.1f);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(90, enemy.health.GetCurrentHealth());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_DealingDoubleDamageWithSmallShield()
        {
            BattleStatusManager player = GetStatusManager(100);
            BattleStatusManager enemy = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            var ability2 = GetAbilityByIndex(0);
            var ability3 = GetAbilityByIndex(3);
            yield return ability3.ExecuteAbility(new BattleStatusManager[] { enemy }, new BattleStatusManager[] { enemy }, 1, null);
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            yield return ability2.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { enemy }, 1, null);
            Assert.AreEqual(85, enemy.health.GetCurrentHealth());
            yield return null;
        }
    }
}