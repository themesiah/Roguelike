using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Float Add")]
    public class FloatAddEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override float ModificationStrategy(float originalValue)
        {
            return originalValue + value;
        }
    }
}