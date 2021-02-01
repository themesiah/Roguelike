using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;

namespace Laresistance.Equipments
{
    [CreateAssetMenu(menuName = "Laresistance/Data/Equipments/Equipment Effect")]
    public class EquipmentEffect : ScriptableObject
    {
        [SerializeField]
        private EquipmentSituationCondition condition = default;
        public EquipmentSituationCondition Condition { get { return condition; } }
        [SerializeField]
        private EquipmentStrategy[] strategies = default;
        public EquipmentStrategy[] Strategies { get { return strategies; } }

        private bool IsSatisfiedBy(EquipmentSituation cond)
        {
            return Condition.IsSatisfiedBy(cond);
        }

        public int ModifyValue(EquipmentSituation cond, int originalValue, int priority)
        {
            int value = originalValue;
            if (IsSatisfiedBy(cond))
            {
                foreach (var strategy in strategies)
                {
                    if (strategy.Priority == priority)
                    {
                        value = strategy.ModificationStrategy(value);
                    }
                }
                
            }
            return value;
        }

        public float ModifyValue(EquipmentSituation cond, float originalValue, int priority)
        {
            float value = originalValue;
            if (IsSatisfiedBy(cond))
            {
                foreach (var strategy in strategies)
                {
                    if (strategy.Priority == priority)
                    {
                        value = strategy.ModificationStrategy(value);
                    }
                }
            }
            return value;
        }

        public ScriptableIntReference ModifyValue(EquipmentSituation cond, ScriptableIntReference reference, int priority)
        {
            if (IsSatisfiedBy(cond))
            {
                foreach (var strategy in strategies)
                {
                    if (strategy.Priority == priority)
                    {
                        strategy.ModificationStrategy(reference);
                    }
                }
            }
            return reference;
        }
    }
}