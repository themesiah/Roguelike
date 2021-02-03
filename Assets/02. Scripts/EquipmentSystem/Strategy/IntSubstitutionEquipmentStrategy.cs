using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Strategy/Int Substitution")]
    public class IntSubstitutionEquipmentStrategy : EquipmentStrategy
    {
        [SerializeField]
        private int value = default;

        public override int ModificationStrategy(int originalValue)
        {
            return value;
        }
    }
}