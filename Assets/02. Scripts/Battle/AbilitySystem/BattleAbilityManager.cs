using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;
using UnityEngine.Analytics;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Battle
{
    public class BattleAbilityManager
    {
        private static bool currentlyExecuting = false;
        private static List<BattleAbility> abilityQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private static IBattleAnimator currentAnimator = null;
        public static BattleAbility currentAbility;
        public static bool executingBasicSkill = false;
        public static BattleStatusManager[] currentTargets;

        private static bool battling = false;
        public static bool Battling => battling;

        public static IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, string animationTrigger, ScriptableIntReference bloodRef)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();

            abilityQueue.Add(abilityToExecute);

            bool stillExecuting = true;
            while (abilityQueue.Count > 0 && abilityQueue[0] != abilityToExecute && (!abilityToExecute.IsPrioritary() || currentAnimator == animator))
            {
                if (abilityQueue.Count == 0)
                {
                    stillExecuting = false;
                }
                yield return null;
            }
            if (abilityQueue.Count == 0)
            {
                stillExecuting = false;
            }
            if (stillExecuting)
            {
                IBattleAnimator lastAnimator = currentAnimator;
                bool needPause = abilityQueue[0] != abilityToExecute;
                if (abilityToExecute.IsPrioritary() && needPause)
                {
                    lastAnimator?.Pause();
                }
                currentlyExecuting = true;
                if (abilityToExecute.IsBasicSkill)
                {
                    executingBasicSkill = true;
                }
                if (abilityToExecute.IsOffensiveAbility)
                {
                    currentTargets = targets;
                }
                currentAnimator = animator;
                currentAbility = abilityToExecute;
                if (targets.Length == 0 || (targets.Length == 1 && targets[0] == null) || (targets.Length >= 1 && targets[0].health.GetCurrentHealth() <= 0))
                {
                    abilityToExecute.CancelByTargetDeath();
                }
                else
                {
                    yield return animator?.PlayAnimation(animationTrigger);
                    if (battling)
                    {
                        yield return abilityToExecute.Perform(allies, targets, level, animator, bloodRef);
                    }
                }
                currentAbility = null;
                abilityQueue.Remove(abilityToExecute);
                currentTargets = null;
                currentlyExecuting = false;
                if (abilityToExecute.IsBasicSkill)
                {
                    executingBasicSkill = false;
                }
                if (abilityToExecute.IsPrioritary() && needPause)
                {
                    lastAnimator?.Resume();
                    currentlyExecuting = true;
                    currentAnimator = lastAnimator;
                }
            }
        }

        public static void StartBattle()
        {
            battling = true;
        }

        public static void StopBattle()
        {
            CancelAllExecutions();
            battling = false;
            currentAnimator?.Stop();
            AbilityInQueue = false;
        }

        public static void CancelAllExecutions()
        {
            if (currentAbility != null)
            {
                CancelExecution(currentAbility);
            }
            abilityQueue.Clear();
        }

        public static void CancelExecution(BattleAbility abilityToCancel)
        {
            if (abilityQueue == null || !abilityQueue.Contains(abilityToCancel))
                return;
                //throw new System.Exception("The ability has not been executed, so it can't be cancelled");

            abilityQueue.Remove(abilityToCancel);
        }

        public static bool Executing => currentlyExecuting;
        public static bool AbilityInQueue = false;
    }
}