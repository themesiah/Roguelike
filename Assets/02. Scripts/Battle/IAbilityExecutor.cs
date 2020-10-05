using System.Collections;

namespace Laresistance.Battle
{
    public interface IAbilityExecutor
    {
        IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies);
    }
}