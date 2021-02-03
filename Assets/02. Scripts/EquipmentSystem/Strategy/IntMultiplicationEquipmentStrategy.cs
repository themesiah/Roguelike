using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Multiplication")]
    public class IntMultiplicationEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;
        [SerializeField]
        private bool ceil = default;

        public override int ModificationStrategy(int originalValue)
        {
            if (ceil)
            {
                return Mathf.CeilToInt(originalValue * value);
            }
            else
            {
                return (int)(originalValue * value);
            }
        }
    }
}