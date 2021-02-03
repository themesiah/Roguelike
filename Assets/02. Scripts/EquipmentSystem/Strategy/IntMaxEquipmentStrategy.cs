using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Max")]
    public class IntMaxEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int max = default;

        public override int ModificationStrategy(int originalValue)
        {
            return System.Math.Min(max, originalValue);
        }
    }
}