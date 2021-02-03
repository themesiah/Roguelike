using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Add")]
    public class IntAddEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;
        [SerializeField]
        private bool cantBeZero = true;

        public override int ModificationStrategy(int originalValue)
        {
            if (cantBeZero)
            {
                return System.Math.Max(0, originalValue + value);
            } else
            {
                return originalValue + value;
            }
        }
    }
}