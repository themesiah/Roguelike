using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Add")]
    public class IntAddEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;

        public override int ModificationStrategy(int originalValue)
        {
            return originalValue + value;
        }
    }
}