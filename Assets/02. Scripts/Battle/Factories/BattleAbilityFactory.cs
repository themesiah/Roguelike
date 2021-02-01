using UnityEngine;
using Laresistance.Data;
using Laresistance.Core;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class BattleAbilityFactory
    {
        public static BattleAbility GetBattleAbility(AbilityData abilityData, EquipmentsContainer equipments, BattleStatusManager battleStatus)
        {
            UnityEngine.Assertions.Assert.IsNotNull(equipments);
            List<BattleEffect> effects = new List<BattleEffect>();
            bool shield = false;
            bool offensive = false;

            foreach(EffectData effectData in abilityData.EffectsData)
            {
                effects.Add(BattleEffectFactory.GetBattleEffect(effectData, battleStatus));
                if (effectData.EffectType == EffectType.Shield)
                {
                    shield = true;
                }
                if (effectData.EffectType == EffectType.Damage || effectData.EffectType == EffectType.BasicAttack)
                {
                    offensive = true;
                }
            }
            effects[0].SetAnimationPrimaryEffect();

            BattleAbility battleAbility = new BattleAbility(effects, abilityData.Cost, abilityData.Weight, abilityData.Cooldown, battleStatus, equipments, abilityData.Icon, abilityData);
            if (shield)
            {
                battleAbility.SetShieldAbility();
            }
            if (offensive)
            {
                battleAbility.SetOffensiveAbility();
            }
            if (abilityData.IsBasicSkill)
            {
                battleAbility.SetBasicSkill();
            }
            return battleAbility;
        }
    }
}