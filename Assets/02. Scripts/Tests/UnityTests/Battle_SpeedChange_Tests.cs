using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;

namespace Laresistance.Tests {
    public class Battle_SpeedChange_Tests
    {
        private BattleAbility GetAbilityByIndex(int i)
        {
            float cooldown = 0f;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch(i)
            {
                case 0:
                    effects.Add(new BattleEffectHeal(10));
                    cooldown = 1f;
                    break;
                case 1:
                    effects.Add(new BattleEffectSpeed(50));
                    cooldown = 1f;
                    break;
                case 2:
                    effects.Add(new BattleEffectSpeed(200));
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
        public IEnumerator When_DoingMultipleAbilitiesNoSpeedChange()
        {
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(0);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
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
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            yield return ability2.ExecuteAbility(enemy, new BattleStatusManager[] { player }, 1);
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
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            yield return ability2.ExecuteAbility(enemy, new BattleStatusManager[] { player }, 1);
            ability.Tick(0.6f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            ability.Tick(0.2f * player.GetSpeedModifier());
            Assert.IsFalse(ability.CanBeUsed());
            yield return null;
        }

        
    }
}