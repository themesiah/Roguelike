using Laresistance.Battle;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Core
{
    public class ConsumableFactory
    {
        public static Consumable GetConsumable(ConsumableData consumableData, EquipmentEvents events, BattleStatusManager battleStatus)
        {
            UnityEngine.Assertions.Assert.IsNotNull(consumableData.AbilityData);
            BattleAbility ability = BattleAbilityFactory.GetBattleAbility(consumableData.AbilityData, events, battleStatus);
            Consumable consumable = new Consumable(consumableData, ability);
            return consumable;
        }
    }
}