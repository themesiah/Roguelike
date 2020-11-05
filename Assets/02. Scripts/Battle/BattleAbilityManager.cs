using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;
using UnityEngine.Analytics;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Battle
{
    public class BattleAbilityManager : MonoBehaviour
    {
        public static bool currentlyExecuting = false;
        private static List<BattleAbility> abilityQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private static IBattleAnimator currentAnimator = null;

        public static IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, string animationTrigger, ScriptableIntReference bloodRef)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();

            abilityQueue.Add(abilityToExecute);

            while (abilityQueue[0] != abilityToExecute && (!abilityToExecute.IsPrioritary() || currentAnimator == animator))
            {
                yield return null;
            }
            IBattleAnimator lastAnimator = currentAnimator;
            bool needPause = abilityQueue[0] != abilityToExecute;
            if (abilityToExecute.IsPrioritary() && needPause)
            {
                lastAnimator?.Pause();
            }
            currentlyExecuting = true;
            currentAnimator = animator;
            yield return animator?.PlayAnimation(animationTrigger);
            abilityToExecute.Perform(allies, targets, level, bloodRef);
            abilityQueue.Remove(abilityToExecute);
            currentlyExecuting = false;
            if (abilityToExecute.IsPrioritary() && needPause)
            {
                lastAnimator?.Resume();
                currentAnimator = lastAnimator;
            }
        }

        public static void CancelExecution(BattleAbility abilityToCancel)
        {
            if (abilityQueue == null || !abilityQueue.Contains(abilityToCancel))
                throw new System.Exception("The ability has not been executed, so it can't be cancelled");

            abilityQueue.Remove(abilityToCancel);
        }

        public static bool Executing => currentlyExecuting;
    }
}