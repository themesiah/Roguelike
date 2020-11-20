﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;
using UnityEngine.Analytics;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Battle
{
    public class BattleAbilityManager
    {
        public static bool currentlyExecuting = false;
        private static List<BattleAbility> abilityQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private static IBattleAnimator currentAnimator = null;
        public static BattleAbility currentAbility;

        private static bool battling = false;

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
            currentAbility = abilityToExecute;
            yield return animator?.PlayAnimation(animationTrigger);
            if (battling)
                abilityToExecute.Perform(allies, targets, level, bloodRef);
            currentAbility = null;
            abilityQueue.Remove(abilityToExecute);
            currentlyExecuting = false;
            if (abilityToExecute.IsPrioritary() && needPause)
            {
                lastAnimator?.Resume();
                currentAnimator = lastAnimator;
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
                throw new System.Exception("The ability has not been executed, so it can't be cancelled");

            abilityQueue.Remove(abilityToCancel);
        }

        public static bool Executing => currentlyExecuting;
    }
}