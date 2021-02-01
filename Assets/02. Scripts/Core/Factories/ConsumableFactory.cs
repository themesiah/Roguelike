using Laresistance.Battle;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Core
{
    public class ConsumableFactory
    {
        public static Consumable GetConsumable(ConsumableData consumableData, EquipmentsContainer equipments, BattleStatusManager battleStatus)
        {
            UnityEngine.Assertions.Assert.IsNotNull(consumableData.AbilityData);
            UnityEngine.Assertions.Assert.IsNotNull(equipments);
            BattleAbility ability = BattleAbilityFactory.GetBattleAbility(consumableData.AbilityData, equipments, battleStatus);
            Consumable consumable = new Consumable(consumableData, ability);
            return consumable;
        }
    }
}