using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Float Sbustitution")]
    public class FloatSubstitutionEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private float value = default;

        public override float ModificationStrategy(float originalValue)
        {
            return value;
        }
    }
}