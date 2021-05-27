using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Battle
{
    public class BattleAbilityManager : MonoBehaviour
    {
        [SerializeField]
        private bool logDebug = false;

        public static BattleAbilityManager Instance;

        private bool currentlyExecuting = false;
        private List<BattleAbility> abilityQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private IBattleAnimator currentAnimator = null;
        [HideInInspector]
        public BattleAbility currentAbility;
        [HideInInspector]
        public bool executingBasicSkill = false;
        [HideInInspector]
        public BattleStatusManager[] currentTargets;
        private BattleAbility executingAbilityCheckPriority;

        public bool Executing => currentlyExecuting;
        [HideInInspector]
        public bool AbilityInQueue = false;
        private bool battling = false;
        public bool Battling => battling;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Log(string message, BattleAbility abilityToExecute, string animationTrigger)
        {
            if (logDebug)
                Debug.LogFormat("[BattleAbilityManager] {0}. (with ability {1} and trigger {2})", message, abilityToExecute.data.name, animationTrigger);
        }

        public IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, string animationTrigger, ScriptableIntReference bloodRef)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();

            if (abilityToExecute.IsPrioritary() && abilityQueue.Count > 0)
            {
                abilityQueue.Insert(1, abilityToExecute);
                Log("Added as prioritary", abilityToExecute, animationTrigger);
            }
            else
            {
                abilityQueue.Add(abilityToExecute);
                Log("Added with normal priority", abilityToExecute, animationTrigger);
            }

            bool stillExecuting = true;
            Log("Waiting to queue", abilityToExecute, animationTrigger);
            while (abilityQueue.Count > 0 && abilityQueue[0] != abilityToExecute && (!abilityToExecute.IsPrioritary() || currentAnimator == animator))
            {
                if (abilityQueue.Count == 0)
                {
                    stillExecuting = false;
                    Log("Ability disappeared from queue? (1)", abilityToExecute, animationTrigger);
                }
                yield return null;
            }
            Log("Ability is next in queue", abilityToExecute, animationTrigger);
            if (abilityQueue.Count == 0)
            {
                Log("Ability disappeared from queue? (2)", abilityToExecute, animationTrigger);
                stillExecuting = false;
            }
            if (stillExecuting)
            {
                Log("Still executing. setting animator and needPause", abilityToExecute, animationTrigger);
                IBattleAnimator lastAnimator = currentAnimator;
                bool needPause = abilityQueue[0] != abilityToExecute;
                if (abilityToExecute.IsPrioritary() && needPause)
                {
                    Log("Pausing last animator, as the ability is prioritary and there is another ability on queue", abilityToExecute, animationTrigger);
                    lastAnimator?.Pause();
                    executingAbilityCheckPriority = abilityQueue[0];
                }
                currentlyExecuting = true;
                if (abilityToExecute.IsBasicSkill)
                {
                    executingBasicSkill = true;
                }
                if (abilityToExecute.IsOffensiveAbility)
                {
                    Log("Setting current targets", abilityToExecute, animationTrigger);
                    currentTargets = targets;
                }
                currentAnimator = animator;
                currentAbility = abilityToExecute;
                if ((targets.Length == 0 || (targets.Length == 1 && targets[0] == null) || (targets.Length >= 1 && targets[0].health.GetCurrentHealth() <= 0))
                    && abilityToExecute.IsInListOfTargets(allies, targets, targets))
                // IsInListOfTargets returns if a BattleStatusManager or a set of them is inside the list of targets an ability have. All abilities have targets, but some
                // may only target oneself (heals). So if you are trying to cancel an ability which doesn't even target the dead enemy, you can't!
                {
                    Log("Cancelling ability because of target death", abilityToExecute, animationTrigger);
                    abilityToExecute.CancelByTargetDeath();
                }
                else
                {
                    Log("Starting ability animation", abilityToExecute, animationTrigger);
                    yield return animator?.PlayAnimation(animationTrigger);
                    if (battling)
                    {
                        Log("Performing ability", abilityToExecute, animationTrigger);
                        yield return abilityToExecute.Perform(allies, targets, level, animator, bloodRef);
                    }
                }
                Log("Ending process. Nullifying variables and removing ability from queue", abilityToExecute, animationTrigger);
                currentAbility = null;
                abilityQueue.Remove(abilityToExecute);
                currentTargets = null;
                currentlyExecuting = false;
                Log("Ability removed from queue", abilityToExecute, animationTrigger);
                if (abilityToExecute.IsBasicSkill)
                {
                    executingBasicSkill = false;
                }
                Log("Checking if there is need to resume the last animator", abilityToExecute, animationTrigger);
                if (abilityToExecute.IsPrioritary() && needPause)
                {
                    Log("Resuming last animator", abilityToExecute, animationTrigger);
                    lastAnimator?.Resume();
                    if (abilityQueue.Count > 0 && executingAbilityCheckPriority == abilityQueue[0])
                    {
                        Log("There are other abilities in queue, so currentlyExecuting must be true", abilityToExecute, animationTrigger);
                        currentlyExecuting = true;
                    }
                    currentAnimator = lastAnimator;
                }
            }
            Log("Finished ability execution", abilityToExecute, animationTrigger);
        }

        public void StartBattle()
        {
            battling = true;
        }

        public void StopBattle()
        {
            CancelAllExecutions();
            battling = false;
            currentAnimator?.Stop();
            AbilityInQueue = false;
        }

        public void CancelAllExecutions()
        {
            if (currentAbility != null)
            {
                CancelExecution(currentAbility);
            }
            abilityQueue.Clear();
        }

        public void CancelExecution(BattleAbility abilityToCancel)
        {
            if (abilityQueue == null || !abilityQueue.Contains(abilityToCancel))
                return;
                //throw new System.Exception("The ability has not been executed, so it can't be cancelled");

            abilityQueue.Remove(abilityToCancel);
        }
    }
}