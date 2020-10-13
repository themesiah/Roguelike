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
        public delegate IEnumerator AnimationToExecuteHandler();
        public static IEnumerator DummyAnimation(){yield return null;}
        public static AnimationToExecuteHandler handler = DummyAnimation;

        public static IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, AnimationToExecuteHandler animation)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();

            abilityQueue.Add(abilityToExecute);

            while(abilityQueue[0] != abilityToExecute)
            {
                yield return null;
            }
            currentlyExecuting = true;
            yield return animation();
            abilityQueue[0].Perform(allies, targets, level);
            abilityQueue.RemoveAt(0);
            currentlyExecuting = false;
        }

        public static void CancelExecution(BattleAbility abilityToCancel)
        {
            if (abilityQueue == null || !abilityQueue.Contains(abilityToCancel))
                throw new System.Exception("The ability has not been executed, so it can't be cancelled");

            abilityQueue.Remove(abilityToCancel);
        }
    }
}