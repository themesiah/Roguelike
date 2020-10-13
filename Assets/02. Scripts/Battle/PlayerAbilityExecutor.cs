using System.Collections;
using Laresistance.Behaviours;
using Laresistance.Core;

namespace Laresistance.Battle
{
    public class PlayerAbilityExecutor : IAbilityExecutor
    {
        private Player player;
        private AnimatorWrapperBehaviour animator;

        public PlayerAbilityExecutor(Player p, AnimatorWrapperBehaviour animator)
        {
            player = p;
            this.animator = animator;
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex == 0)
            {
                yield return player.characterAbility.ExecuteAbility(allies, enemies, 1, animator);
            } else if (abilityIndex > 0 && abilityIndex  < 4)
            {
                yield return player.GetMinions()[abilityIndex-1].ExecuteAbility(allies, enemies, animator);
            } else
            {
                yield return player.GetConsumables()[abilityIndex - 4].Ability.ExecuteAbility(allies, enemies, 1, animator);
            }
        }
    }
}