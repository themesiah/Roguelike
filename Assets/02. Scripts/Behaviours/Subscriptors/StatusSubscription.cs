using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Laresistance.Data;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Pools;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class StatusSubscription : MonoBehaviour
    {
        [System.Serializable]
        public class StatusSubscriptionEvent
        {
            [SerializeField]
            private StatusIconType statusType = default;
            [SerializeField]
            private UnityEvent subscriptionEvent = default;

            public bool IsStatusType(StatusIconType type) => this.statusType == type;
            public void Invoke() => subscriptionEvent?.Invoke();
        }
        [SerializeField]
        private StatusList statusList = default;
        [SerializeField]
        private StatusSubscriptionEvent[] OnStartStatusEvents = default;
        [SerializeField]
        private StatusSubscriptionEvent[] OnEndStatusEvents = default;
        [SerializeField]
        private Transform statusIndicatorsHolder = default;
        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;
        [SerializeField]
        private UnityEvent OnAbilitiesNeedUpdate = default;
        [SerializeField]
        private UnityEvent OnStunned = default;
        [SerializeField]
        private UnityEvent OnSlowed = default;
        [SerializeField]
        private UnityEvent OnPoisoned = default;
        [SerializeField]
        private UnityEvent OnWarded = default;

        private bool haveShields = false;
        private bool haveDamageImprovement = false;
        private ScriptablePool statusIndicatorPool;

        private List<StatusIconManager> buffList;
        private List<StatusIconManager> debuffList;
        private List<StatusIconManager> otherList;

        private void Awake()
        {
            statusIndicatorPool = PoolInitializerBehaviour.GetPool("Status");
            buffList = new List<StatusIconManager>();
            debuffList = new List<StatusIconManager>();
            otherList = new List<StatusIconManager>();
        }

        private void OnEnable()
        {
            battleBehaviour.StatusManager.OnStatusApplied += OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus += OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged += OnShieldsChanged;
            battleBehaviour.StatusManager.OnBuffsRemoved += OnRemoveBuffs;
            battleBehaviour.StatusManager.OnDebuffsRemoved += OnRemoveDebuffs;
            battleBehaviour.StatusManager.OnSingletonStatusRemoved += OnRemoveSingleton;
            battleBehaviour.StatusManager.OnStatusWarded += OnStatusWarded;
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.OnStatusApplied -= OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus -= OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged -= OnShieldsChanged;
            battleBehaviour.StatusManager.OnBuffsRemoved -= OnRemoveBuffs;
            battleBehaviour.StatusManager.OnDebuffsRemoved -= OnRemoveDebuffs;
            battleBehaviour.StatusManager.OnSingletonStatusRemoved -= OnRemoveSingleton;
            battleBehaviour.StatusManager.OnStatusWarded -= OnStatusWarded;
        }

        private void OnStatusApplied(BattleStatusManager sender, StatusIconType statusType, float duration)
        {
            OnStatusApplied(statusType, duration);
        }

        private void OnStatusApplied(StatusIconType statusType, float duration)
        {
            if (statusType == StatusIconType.Buff || statusType == StatusIconType.DamageImprovement || statusType == StatusIconType.Debuff)
            {
                OnAbilitiesNeedUpdate?.Invoke();
            }
            if ((statusType == StatusIconType.Shield && haveShields) || (statusType == StatusIconType.DamageImprovement && haveDamageImprovement))
            {
                return;
            }

            switch(statusType)
            {
                case StatusIconType.Stun:
                    OnStunned?.Invoke();
                    break;
                case StatusIconType.Slow:
                    OnSlowed?.Invoke();
                    break;
                case StatusIconType.DoT:
                    OnPoisoned?.Invoke();
                    break;
            }

            StatusData status = GetStatus(statusType);
            GameObject statusIconObject = statusIndicatorPool.GetInstance(instanceParent: statusIndicatorsHolder);
            statusIconObject.transform.localScale = Vector3.one;
            StatusIconManager sim = statusIconObject.GetComponent<StatusIconManager>();
            sim.InitStatusIcon(statusType, status.StatusFrame, status.StatusSprite, status.StatusFrameColor, duration, battleBehaviour.StatusManager, statusIndicatorPool);
            sim.OnStatusTerminated += OnStatusTerminated;
            if (statusType == StatusIconType.Shield)
            {
                haveShields = true;
            }

            if (statusType == StatusIconType.DamageImprovement)
            {
                haveDamageImprovement = true;
            }

            if (statusType == StatusIconType.Blind || statusType == StatusIconType.Debuff || statusType == StatusIconType.DoT || statusType == StatusIconType.DoTFire || statusType == StatusIconType.Slow || statusType == StatusIconType.Stun)
            {
                debuffList.Add(sim);
            } else if (statusType == StatusIconType.Buff || statusType == StatusIconType.DamageImprovement || statusType == StatusIconType.Speed || statusType == StatusIconType.ParryPrepared || statusType == StatusIconType.ShieldPrepared)
            {
                buffList.Add(sim);
            }
            else
            {
                otherList.Add(sim);
            }
            ExecuteOnStartEvents(statusType);
        }

        private void OnShieldsChanged(CharacterHealth sender, int delta, int total, bool isDamage, float shieldPercent)
        {
            if (total > 0)
            {
                OnStatusApplied(StatusIconType.Shield, -1f);
            }
        }

        private void OnResetStatus(BattleStatusManager sender)
        {
            haveShields = false;
            haveDamageImprovement = false;
        }

        private StatusData GetStatus(StatusIconType statusType)
        {
            foreach(var status in statusList.Statuses)
            {
                if (statusType == status.Status)
                {
                    return status;
                }
            }
            return null;
        }

        private void OnStatusTerminated(StatusIconManager sender, StatusIconType type)
        {
            if (type == StatusIconType.Shield)
            {
                haveShields = false;
            }

            if (type == StatusIconType.DamageImprovement)
            {
                haveDamageImprovement = false;
            }

            sender.OnStatusTerminated -= OnStatusTerminated;
            if (buffList.Contains(sender))
            {
                buffList.Remove(sender);
            }
            if (debuffList.Contains(sender))
            {
                debuffList.Remove(sender);
            }
            if (otherList.Contains(sender))
            {
                otherList.Remove(sender);
            }
            ExecuteOnEndEvents(type);
        }

        private void OnRemoveBuffs(BattleStatusManager statusManager)
        {
            for (int i = buffList.Count-1; i >= 0; --i)
            {
                var sim = buffList[i];
                sim.ManualTermination();
            }
            buffList.Clear();
        }

        private void OnRemoveDebuffs(BattleStatusManager statusManager)
        {
            for (int i = debuffList.Count-1; i >= 0; --i)
            {
                var sim = debuffList[i];
                sim.ManualTermination();
            }
            debuffList.Clear();
        }

        private void OnRemoveSingleton(BattleStatusManager statusManager, StatusIconType statusType)
        {
            for (int i = 0; i < buffList.Count; ++i)
            {
                if (buffList[i].GetStatusType == statusType)
                {
                    buffList[i].ManualTermination();
                    break;
                }
            }
            for (int i = 0; i < debuffList.Count; ++i)
            {
                if (debuffList[i].GetStatusType == statusType)
                {
                    debuffList[i].ManualTermination();
                    break;
                }
            }
            for (int i = 0; i < otherList.Count; ++i)
            {
                if (otherList[i].GetStatusType == statusType)
                {
                    otherList[i].ManualTermination();
                    break;
                }
            }
        }

        private void ExecuteOnStartEvents(StatusIconType statusType)
        {
            foreach(var ev in OnStartStatusEvents)
            {
                if (ev.IsStatusType(statusType))
                    ev.Invoke();
            }
        }

        private void ExecuteOnEndEvents(StatusIconType statusType)
        {
            foreach (var ev in OnEndStatusEvents)
            {
                if (ev.IsStatusType(statusType))
                    ev.Invoke();
            }
        }

        private void OnStatusWarded(BattleStatusManager statusManager)
        {
            OnWarded?.Invoke();
        }
    }
}