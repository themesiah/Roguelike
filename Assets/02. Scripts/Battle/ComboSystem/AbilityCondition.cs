using Laresistance.Patterns;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class AbilityCondition : CompositeSpecification<BattleAbility>
    {
        private AbilityConditionData abilityConditionData;
        public int ConditionLength => abilityConditionData.AbilityDatas.Length;
        public AbilityCondition(AbilityConditionData abilityConditionData)
        {
            this.abilityConditionData = abilityConditionData;
        }

        public override bool IsSatisfiedBy(BattleAbility candidate)
        {
            SingleAbilityCondition[] abilityConditions = new SingleAbilityCondition[ConditionLength];
            for (int i = 0; i < ConditionLength; ++i)
            {
                var abilityData = abilityConditionData.AbilityDatas[i];
                abilityConditions[i] = new SingleAbilityCondition(abilityData);
            }
            return new OrMultiSpecification<BattleAbility>(abilityConditions).IsSatisfiedBy(candidate);
        }
    }
}