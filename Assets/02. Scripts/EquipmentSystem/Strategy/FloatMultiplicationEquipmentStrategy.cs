using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Float Multiplication")]
    public class FloatMultiplicationEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override float ModificationStrategy(float originalValue)
        {
            return originalValue * value;
        }
    }
}