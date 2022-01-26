using System.Collections.Generic;

namespace Laresistance.Battle
{
    public interface IAbilityInputProcessor : ITimeStoppable
    {
        AbilityExecutionData GetAbilitiesToExecute(BattleStatusManager battleStatus, float delta, float unmodifiedDelta);
        BattleAbility[] GetAbilities();
        void BattleStart();
        void BattleEnd();
        void SetAdvantage();
    }
}