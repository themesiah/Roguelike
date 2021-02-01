using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Multiplication")]
    public class IntMultiplicationEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override int ModificationStrategy(int originalValue)
        {
            return (int)(originalValue * value);
        }
    }
}