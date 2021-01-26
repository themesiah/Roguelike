using Laresistance.Data;
using Laresistance.Patterns;

namespace Laresistance.Battle
{
    public class SingleAbilityCondition : CompositeSpecification<BattleAbility>
    {
        private AbilityData abilityData;

        public SingleAbilityCondition(AbilityData abilityData)
        {
            this.abilityData = abilityData;
        }

        public override bool IsSatisfiedBy(BattleAbility candidate)
        {
            return candidate.data == abilityData;
        }
    }
}