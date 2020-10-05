using System.Collections;
using Laresistance.Core;

namespace Laresistance.Battle
{
    public class PlayerAbilityExecutor : IAbilityExecutor
    {
        private Player player;

        public PlayerAbilityExecutor(Player p)
        {
            player = p;
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex == 0)
            {
                yield return player.characterAbility.ExecuteAbility(allies, enemies, 1);
            } else if (abilityIndex > 0 && abilityIndex  < 4)
            {
                yield return player.GetMinions()[abilityIndex-1].ExecuteAbility(allies, enemies);
            } else
            {
                yield return player.GetConsumables()[abilityIndex - 4].Ability.ExecuteAbility(allies, enemies, 1);
            }
        }
    }
}