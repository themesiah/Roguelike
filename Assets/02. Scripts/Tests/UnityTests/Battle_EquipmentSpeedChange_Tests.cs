using NUnit.Framework;
using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TestTools;
using UnityEngine;
using Laresistance.Core;

namespace Laresistance.Tests {
    public class Battle_EquipmentSpeedChange_Tests
    {
        private BattleAbility GetAbilityByIndex(int i, EquipmentEvents events)
        {
            float cooldown = 0f;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch (i)
            {
                case 0:
                    effects.Add(new BattleEffectHeal(10));
                    cooldown = 1f;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cooldown, events);
            ability.Tick(1.1f);
            return ability;
        }

        private BattleStatusManager GetStatusManager(int health)
        {
            return new BattleStatusManager(new CharacterHealth(health));
        }

        [UnityTest]
        public IEnumerator When_ReducingChargeSpeed()
        {
            EquipmentEvents events = new EquipmentEvents();
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(0, events);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e = new Equipment(0);
            e.SetCooldownModifier(events, 2f);
            e.EquipEquipment();
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsFalse(ability.CanBeUsed());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_MaintainingChargeSpeed()
        {
            EquipmentEvents events = new EquipmentEvents();
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(0, events);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e = new Equipment(0);
            e.SetCooldownModifier(events, 2f);
            e.EquipEquipment();
            Equipment e2 = new Equipment(1);
            e2.SetCooldownModifier(events, 0.5f);
            e2.EquipEquipment();
            ability.Tick(1.1f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return null;
        }

        [UnityTest]
        public IEnumerator When_ImprovingChargeSpeed()
        {
            EquipmentEvents events = new EquipmentEvents();
            BattleStatusManager player = GetStatusManager(100);
            var ability = GetAbilityByIndex(0, events);
            Assert.IsTrue(ability.CanBeUsed());
            yield return ability.ExecuteAbility(player, new BattleStatusManager[] { player }, 1);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e2 = new Equipment(1);
            e2.SetCooldownModifier(events, 0.5f);
            e2.EquipEquipment();
            ability.Tick(0.55f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return null;
        }
    }
}