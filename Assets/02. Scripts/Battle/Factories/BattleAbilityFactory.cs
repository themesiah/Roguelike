using UnityEngine;
using Laresistance.Data;
using Laresistance.Core;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class BattleAbilityFactory
    {
        public static BattleAbility GetBattleAbility(AbilityData abilityData, EquipmentEvents events)
        {
            List<BattleEffect> effects = new List<BattleEffect>();

            foreach(EffectData effectData in abilityData.EffectsData)
            {
                effects.Add(BattleEffectFactory.GetBattleEffect(effectData));
            }

            BattleAbility battleAbility = new BattleAbility(effects, abilityData.Cooldown, events);
            return battleAbility;
        }
    }
}