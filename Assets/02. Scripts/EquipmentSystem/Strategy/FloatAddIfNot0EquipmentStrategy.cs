using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Float Add If Not 0")]
    public class FloatAddIfNot0EquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override float ModificationStrategy(float originalValue)
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