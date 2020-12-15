using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using UnityEngine;

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
            if (abilityIndex >= 0 && abilityIndex < 4)
            {
                yield return player.characterAbilities[abilityIndex].ExecuteAbility(allies, enemies, 1, animator, bloodRef);
            } else if (abilityIndex >= 4 && abilityIndex  < 16)
            {
                int minionIndex = Mathf.FloorToInt((abilityIndex - 4) / 4);
                int minionAbility = (abilityIndex - 4) % 4;
                yield return player.GetMinions()[minionIndex].ExecuteAbility(minionAbility, allies, enemies, animator, bloodRef);
            } else
            {
                Consumable c = player.GetConsumables()[abilityIndex - 4];
                yield return c.Ability.ExecuteAbility(allies, enemies, 1, animator);
                player.UseConsumable(c);
            }
        }
    }
}