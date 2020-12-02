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
        private BattleStatusManager GetStatus()
        {
            return new BattleStatusManager(new CharacterHealth(100));
        }

        private BattleAbility GetAbilityByIndex(int i, EquipmentEvents events)
        {
            int cost = 0;
            List<BattleEffect> effects = new List<BattleEffect>();

            switch (i)
            {
                case 0:
                    effects.Add(new BattleEffectHeal(10, Data.EffectTargetType.Self, GetStatus()));
                    cost = 1;
                    break;
            }

            BattleAbility ability = new BattleAbility(effects, cost, 0, null, events);
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
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e = new Equipment(0, null, null);
            e.SetEnergyProduction(events, 2f);
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
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e = new Equipment(0, null, null);
            e.SetEnergyProduction(events, 2f);
            e.EquipEquipment();
            Equipment e2 = new Equipment(1, null, null);
            e2.SetEnergyProduction(events, 0.5f);
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
            yield return ability.ExecuteAbility(new BattleStatusManager[] { player }, new BattleStatusManager[] { player }, 1, null);
            Assert.IsFalse(ability.CanBeUsed());
            Equipment e2 = new Equipment(1, null, null);
            e2.SetEnergyProduction(events, 0.5f);
            e2.EquipEquipment();
            ability.Tick(0.55f * player.GetSpeedModifier());
            Assert.IsTrue(ability.CanBeUsed());
            yield return null;
        }
    }
}