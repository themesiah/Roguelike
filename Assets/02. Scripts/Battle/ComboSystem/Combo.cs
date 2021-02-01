using Laresistance.Core;
using Laresistance.Data;
using Laresistance.Patterns;

namespace Laresistance.Battle
{
    public class Combo : ShowableElement, ISpecification<BattleAbility[]>
    {
        public ComboData comboData { get; private set; }
        public BattleAbility comboAbility { get; private set; }
        public ComboCondition comboCondition { get; private set; }
        public int ComboLength { get { return comboCondition.ConditionLength; } }

        public Combo(ComboData comboData, EquipmentsContainer equipments, BattleStatusManager statusManager)
        {
            this.comboData = comboData;
            comboAbility = BattleAbilityFactory.GetBattleAbility(comboData.ComboAbility, equipments, statusManager);
            comboCondition = new ComboCondition(comboData.ComboCondition);
        }

        public bool IsSatisfiedBy(BattleAbility[] candidate)
        {
            return comboCondition.IsSatisfiedBy(candidate);
        }

        public ISpecification<BattleAbility[]> And(ISpecification<BattleAbility[]> other)
        {
            throw new System.NotImplementedException();
        }

        public ISpecification<BattleAbility[]> AndNot(ISpecification<BattleAbility[]> other)
        {
            throw new System.NotImplementedException();
        }

        public ISpecification<BattleAbility[]> Or(ISpecification<BattleAbility[]> other)
        {
            throw new System.NotImplementedException();
        }

        public ISpecification<BattleAbility[]> OrNot(ISpecification<BattleAbility[]> other)
        {
            throw new System.NotImplementedException();
        }

        public ISpecification<BattleAbility[]> Not()
        {
            throw new System.NotImplementedException();
        }
    }
}