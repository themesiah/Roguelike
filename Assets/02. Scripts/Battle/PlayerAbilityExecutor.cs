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

        public IEnumerator ExecuteAbility(int minionIndex, BattleStatusManager user, BattleStatusManager[] enemies)
        {
            if (minionIndex < 0 || minionIndex > player.EquippedMinionsQuantity - 1)
                throw new System.Exception("Invalid index for executing minion ability");
            yield return player.GetMinions()[minionIndex].ExecuteAbility(user, enemies);
        }
    }
}