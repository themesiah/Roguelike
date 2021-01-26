using System.Collections.Generic;

namespace Laresistance.Battle
{
    public interface IAbilityInputProcessor : ITimeStoppable
    {
        AbilityExecutionData GetAbilitiesToExecute(BattleStatusManager battleStatus, float delta);
        BattleAbility[] GetAbilities();
        void BattleStart();
        void BattleEnd();
    }
}