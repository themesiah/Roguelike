using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.Utils;

namespace Laresistance.Battle
{
    public class BattleAbilityManager : MonoBehaviour
    {
        [SerializeField]
        private bool logDebug = false;
        [SerializeField]
        private string logFileName = "BattleAbilityManager.log";

        public static BattleAbilityManager Instance;

        private bool currentlyExecuting = false;
        private List<BattleAbility> abilityQueue;
        private List<IBattleAnimator> animatorQueue;
        private List<BattleStatusManager[]> currentTargetsQueue;
        public delegate IEnumerator AnimationToExecuteHandler(string trigger);
        private Dictionary<BattleAbility, BattleStatusManager> casterMap;

        public bool Executing => currentlyExecuting;
        public BattleAbility currentAbility => abilityQueue == null || abilityQueue.Count == 0 ? null : abilityQueue[0];
        public BattleStatusManager[] currentTargets => currentTargetsQueue == null || currentTargetsQueue.Count == 0 ? null : currentTargetsQueue[0];
        [HideInInspector]
        public bool AbilityInQueue = false;
        private bool battling = false;
        public bool Battling => battling;
        public bool QueueIsEmpty
        {
            get
            {
                if (abilityQueue == null)
                {
                    return true;
                }
                else
                {
                    return abilityQueue.Count == 0;
                }
            }
        }

        private void Awake()
        {
            Instance = this;
            casterMap = new Dictionary<BattleAbility, BattleStatusManager>();
            if (logDebug)
            {
                Utils.DeleteFile(logFileName);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Log(string message, BattleAbility abilityToExecute, string animationTrigger)
        {
            if (logDebug)
            {
                string textToLog = string.Format("{0}. (with ability {1} and trigger {2})", message, abilityToExecute.data.name, animationTrigger);
                Debug.LogFormat("[BattleAbilityManager] {0}.", textToLog);
                Utils.AppendText(logFileName, textToLog);
            }
        }

        public IEnumerator ExecuteAbility(BattleAbility abilityToExecute, BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, string animationTrigger, ScriptableIntReference bloodRef)
        {
            if (abilityQueue == null)
                abilityQueue = new List<BattleAbility>();
            if (animatorQueue == null)
                animatorQueue = new List<IBattleAnimator>();
            if (currentTargetsQueue == null)
                currentTargetsQueue = new List<BattleStatusManager[]>();

            if (abilityToExecute.IsPrioritary() && abilityQueue.Count > 0 && !CasterHasAbilitiesInQueue(allies[0]))
            {
                animatorQueue[0].Pause();
                abilityQueue.Insert(0, abilityToExecute);
                animatorQueue.Insert(0, animator);
                currentTargetsQueue.Insert(0, targets);
                Log("Added as prioritary", abilityToExecute, animationTrigger);
            }
            else if (abilityToExecute.IsPrioritary() && abilityQueue.Count > 0 && CasterHasAbilitiesInQueue(allies[0]))
            {
                int position = GetFirstAbilityInQueue(allies[0]);
                abilityQueue.Insert(position + 1, abilityToExecute);
                animatorQueue.Insert(position + 1, animator);
                currentTargetsQueue.Insert(position + 1, targets);
                Log("Added as a secondary priority in position " + (position+1).ToString(), abilityToExecute, animationTrigger);
            }
            else
            {
                abilityQueue.Add(abilityToExecute);
                animatorQueue.Add(animator);
                currentTargetsQueue.Add(targets);
                Log("Added with normal priority", abilityToExecute, animationTrigger);
            }
            if (casterMap.ContainsKey(abilityToExecute))
            {
                casterMap.Remove(abilityToExecute);
            }
            casterMap.Add(abilityToExecute, allies[0]);

            bool stillExecuting = true;
            Log("Waiting to queue", abilityToExecute, animationTrigger);
            abilityToExecute.SetAbilityState(BattleAbility.AbilityState.WaitingInQueue);
            while ((abilityQueue.Count > 0 && abilityQueue[0] != abilityToExecute)
                || abilityQueue[0].State == BattleAbility.AbilityState.Finished)
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
                abilityToExecute.SetAbilityState(BattleAbility.AbilityState.Executing);
                Log("Still executing. setting animator and needPause", abilityToExecute, animationTrigger);
                currentlyExecuting = true;
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
                    if (animatorQueue.Count > 0)
                        animatorQueue[0].Resume();
                    yield return animator?.PlayAnimation(animationTrigger);
                    abilityToExecute.SetAbilityState(BattleAbility.AbilityState.Finished);
                    if (battling)
                    {
                        Log("Performing ability", abilityToExecute, animationTrigger);
                        yield return abilityToExecute.Perform(allies, targets, level, animator, bloodRef);
                    }
                }
                Log("Ending process. Nullifying variables and removing ability from queue", abilityToExecute, animationTrigger);
                if (battling)
                {
                    // Before, it was abilityQueue?.RemoveAt(0). Due to an extreme case with retaliation damage if an enemy killed itself when attacking you and the battle wasn't finished,
                    // we need to check that the ability actually exists on the queue before removing.
                    if (abilityQueue.Contains(abilityToExecute))
                    {
                        abilityQueue?.Remove(abilityToExecute);
                    }
                    if (abilityQueue.Count == 0)
                    {
                        currentlyExecuting = false;
                    }
                    animatorQueue?.RemoveAt(0);
                    if (animatorQueue.Count > 0)
                    {
                        animatorQueue[0]?.Resume();
                    }
                    currentTargetsQueue?.RemoveAt(0);
                    Log("Ability removed from queue", abilityToExecute, animationTrigger);
                    Log("Checking if there is need to resume the last animator", abilityToExecute, animationTrigger);
                }
            }
            Log("Finished ability execution", abilityToExecute, animationTrigger);
            abilityToExecute.SetAbilityState(BattleAbility.AbilityState.Idle);
            if (casterMap.ContainsKey(abilityToExecute))
            {
                casterMap.Remove(abilityToExecute);
            }
        }

        private bool CasterHasAbilitiesInQueue(BattleStatusManager caster)
        {
            foreach(var ability in abilityQueue)
            {
                if (casterMap.ContainsKey(ability) && casterMap[ability] == caster)
                    return true;
            }
            return false;
        }

        private int GetFirstAbilityInQueue(BattleStatusManager caster)
        {
            for (int i = 0; i < abilityQueue.Count; ++i)
            {
                var ability = abilityQueue[i];
                if (casterMap.ContainsKey(ability) && casterMap[ability] == caster)
                    return i;
            }
            return -1;
        }

        public void StartBattle()
        {
            battling = true;
            casterMap.Clear();
        }

        public void StopBattle()
        {
            CancelAllExecutions();
            battling = false;
            AbilityInQueue = false;
            casterMap.Clear();
            currentlyExecuting = false;
        }

        public void CancelAllExecutions()
        {
            if (abilityQueue.Count > 0)
            {
                CancelExecution();
            }
            abilityQueue.Clear();
            animatorQueue.Clear();
            currentTargetsQueue.Clear();
        }

        public void CancelExecution()
        {
            if (abilityQueue == null)
                return;

            for (int i = 0; i < abilityQueue.Count; ++i)
            {
                abilityQueue[i].SetAbilityState(BattleAbility.AbilityState.Idle);
            }

            for (int i = 0; i < animatorQueue.Count; ++i)
            {
                animatorQueue[i].Stop();
            }
        }

        public void CancelExecution(BattleAbility abilityToCancel)
        {
            if (abilityQueue == null || !abilityQueue.Contains(abilityToCancel))
                return;

            abilityToCancel.SetAbilityState(BattleAbility.AbilityState.Idle);
            abilityQueue.Remove(abilityToCancel);
        }
    }
}