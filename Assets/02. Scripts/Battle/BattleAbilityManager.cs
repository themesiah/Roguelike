using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;

namespace Laresistance.Battle
{
    public class BattleAbilityManager : MonoBehaviour
    {
        public static bool currentlyExecuting = false;
        private static List<BattleAbility> abilityQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private static AnimatorWrapperBehaviour currentAnimator = null;

        public static IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, AnimatorWrapperBehaviour animator, string animationTrigger)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();

            abilityQueue.Add(abilityToExecute);

            while(abilityQueue[0] != abilityToExecute && !abilityToExecute.IsPrioritary())
            {
                yield return null;
            }
            AnimatorWrapperBehaviour lastAnimator = currentAnimator;
            if (abilityToExecute.IsPrioritary())
            {
                lastAnimator?.Pause();
            }
            currentlyExecuting = true;
            currentAnimator = animator;
            yield return animator?.PlayAnimation(animationTrigger);
            abilityToExecute.Perform(allies, targets, level);
            abilityQueue.Remove(abilityToExecute);
            currentlyExecuting = false;
            if (abilityToExecute.IsPrioritary())
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
    }
}