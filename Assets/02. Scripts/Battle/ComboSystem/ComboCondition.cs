using Laresistance.Patterns;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class ComboCondition : CompositeSpecification<BattleAbility[]>
    {
        public ComboConditionData conditionData { get; private set; }

        public int ConditionLength => conditionData.AbilityConditionDatas.Length;

        public ComboCondition(ComboConditionData conditionData)
        {
            this.conditionData = conditionData;
        }

        public override bool IsSatisfiedBy(BattleAbility[] candidate)
        {
            if (candidate.Length < ConditionLength)
                return false;
            for (int i = 0; i < ConditionLength; ++i)
            {
                var abilityData = conditionData.AbilityConditionDatas[i];
                if (!new AbilityCondition(abilityData).IsSatisfiedBy(candidate[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}