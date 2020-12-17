using Laresistance.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class EnemyAbilityManager : IAbilityInputProcessor, IAbilityExecutor
    {
        private static float NEXT_ABILITY_COOLDOWN = 3f;
        private BattleAbility[] abilities;
        private int level;
        private IBattleAnimator animator;

        private BattleAbility nextAbility = null;
        private float nextAbilityTimer = NEXT_ABILITY_COOLDOWN;

        public EnemyAbilityManager(BattleAbility[] abilities, int level, IBattleAnimator animator)
        {
            this.abilities = abilities;
            this.level = level;
            this.animator = animator;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            if (nextAbility == null)
            {
                nextAbility = GetRandomAbilityFromWeights();
                battleStatus.SetNextAbility(nextAbility);
            }

            for(int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;
                abilities[i].Tick(delta);
            }

            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == null)
                    continue;

                if (abilities[i].CanBeUsed() && nextAbilityTimer <= 0f)
                {
                    if (abilities[i] == nextAbility)
                    {
                        nextAbilityTimer = NEXT_ABILITY_COOLDOWN;
                        return i;
                    } else
                    {
                        //if (abilities[i].IsShieldAbility)
                        //{
                        //    BattleAbility currentAbility = BattleAbilityManager.currentAbility;
                        //    if (currentAbility != null && BattleAbilityManager.Executing && currentAbility.GetStatusManager() != battleStatus && currentAbility.IsOffensiveAbility)
                        //    {
                        //        var currentTargets = BattleAbilityManager.currentTargets;
                        //        foreach(var target in currentTargets)
                        //        {
                        //            if (target == battleStatus) // An enemy must be targetted (or its team) to activate the shields
                        //            {
                        //                battleStatus.SetNextAbility(null);
                        //                return i;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            if (!BattleAbilityManager.Executing)
            {
                nextAbilityTimer -= delta;
            }
            return -1;
        }

        public IEnumerator ExecuteAbility(int abilityIndex, BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            if (abilityIndex < 0 || abilityIndex > abilities.Length - 1)
                throw new System.Exception("Invalid index for executing ability");
            yield return abilities[abilityIndex].ExecuteAbility(allies, enemies, level, animator);
            nextAbility = null;
        }

        public BattleAbility[] GetAbilities()
        {
            return abilities;
        }

        public void BattleStart()
        {

        }

        private BattleAbility GetRandomAbilityFromWeights()
        {
            List<BattleAbility> weightedAbilities = new List<BattleAbility>();
            foreach(var ability in abilities)
            {
                if (ability != null)
                {
                    for (int i = 0; i < ability.Weight; ++i)
                    {
                        weightedAbilities.Add(ability);
                    }
                }
            }
            if (weightedAbilities.Count == 0)
            {
                return null; // This should only be the case for an enemy with only a reactive skill, like a shield minion
            }
            return weightedAbilities[Random.Range(0, weightedAbilities.Count)];
        }
    }
}