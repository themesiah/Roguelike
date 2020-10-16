using Laresistance.Behaviours;
using System.Collections;
using UnityEngine;

namespace Laresistance.Battle
{
    public class EnemyAbilityManager : IAbilityInputProcessor, IAbilityExecutor
    {
        private BattleAbility[] abilities;
        private int level;
        private IBattleAnimator animator;

        public EnemyAbilityManager(BattleAbility[] abilities, int level, IBattleAnimator animator)
        {
            this.abilities = abilities;
            this.level = level;
            this.animator = animator;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            for(int i = 0; i < abilities.Length; ++i)
            {
                abilities[i].Tick(delta * battleStatus.GetSpeedModifier());
                if (abilities[i].CanBeUsed())
                {
                    return i;
                }
            }
            return -1;
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex < 0 || abilityIndex > abilities.Length - 1)
                throw new System.Exception("Invalid index for executing ability");
            yield return abilities[abilityIndex].ExecuteAbility(allies, enemies, level, animator);
        }
    }
}