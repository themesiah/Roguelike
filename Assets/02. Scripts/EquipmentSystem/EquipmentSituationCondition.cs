using UnityEngine;
using Laresistance.Patterns;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Equipment Situation Condition")]
    public class EquipmentSituationCondition : ScriptableObject, ISpecification<EquipmentSituation>
    {
        [SerializeField]
        private EquipmentSituation situation = default;
        public EquipmentSituation Situation { get { return situation; } }

        public bool IsSatisfiedBy(EquipmentSituation candidate)
        {
            return situation == candidate;
        }

        public ISpecification<EquipmentSituation> And(ISpecification<EquipmentSituation> other) => new AndSpecification<EquipmentSituation>(this, other);
        public ISpecification<EquipmentSituation> AndNot(ISpecification<EquipmentSituation> other) => new AndNotSpecification<EquipmentSituation>(this, other);
        public ISpecification<EquipmentSituation> Or(ISpecification<EquipmentSituation> other) => new OrSpecification<EquipmentSituation>(this, other);
        public ISpecification<EquipmentSituation> OrNot(ISpecification<EquipmentSituation> other) => new OrNotSpecification<EquipmentSituation>(this, other);
        public ISpecification<EquipmentSituation> Not() => new NotSpecification<EquipmentSituation>(this);
    }
}