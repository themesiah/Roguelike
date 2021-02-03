using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Add If Not 0")]
    public class IntAddIfNot0EquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;

        public override int ModificationStrategy(int originalValue)
        {
            if (originalValue > 0)
            {
                return originalValue + value;
            } else
            {
                return originalValue;
            }
        }
    }
}