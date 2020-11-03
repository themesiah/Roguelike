using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;

namespace Laresistance.Battle
{
    public class PlayerAbilityExecutor : IAbilityExecutor
    {
        private Player player;
        private IBattleAnimator animator;
        private ScriptableIntReference bloodRef;

        public PlayerAbilityExecutor(Player p, IBattleAnimator animator, ScriptableIntReference bloodRef)
        {
            player = p;
            this.animator = animator;
            this.bloodRef = bloodRef;
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex == 0)
            {
                yield return player.characterAbility.ExecuteAbility(allies, enemies, 1, animator, bloodRef);
            } else if (abilityIndex > 0 && abilityIndex  < 4)
            {
                yield return player.GetMinions()[abilityIndex-1].ExecuteAbility(allies, enemies, animator, bloodRef);
            } else
            {
                Consumable c = player.GetConsumables()[abilityIndex - 4];
                yield return c.Ability.ExecuteAbility(allies, enemies, 1, animator);
                player.UseConsumable(c);
            }
        }
    }
}