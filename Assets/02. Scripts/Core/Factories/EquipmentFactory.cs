using Laresistance.Battle;
using Laresistance.Data;

namespace Laresistance.Core
{
    public class EquipmentFactory
    {
        public static Equipment GetEquipment(EquipmentData equipmentData, BattleStatusManager statusManager)
        {
            UnityEngine.Assertions.Assert.IsNotNull(equipmentData.EquipmentEffects);
            UnityEngine.Assertions.Assert.IsTrue(equipmentData.EquipmentEffects.Count > 0);
            Equipment equip = new Equipment(equipmentData, statusManager);
            return equip;
        }
    }
}