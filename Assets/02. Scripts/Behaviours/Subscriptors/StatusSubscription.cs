using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Laresistance.Data;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Pools;

namespace Laresistance.Behaviours
{
    public class StatusSubscription : MonoBehaviour
    {
        [SerializeField]
        private StatusList statusList = default;
        [SerializeField]
        private Transform statusIndicatorsHolder = default;
        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;

        private bool haveShields = false;
        private bool haveDamageImprovement = false;
        private ScriptablePool statusIndicatorPool;

        private List<StatusIconManager> buffList;
        private List<StatusIconManager> debuffList;

        private void Start()
        {
            statusIndicatorPool = PoolInitializerBehaviour.GetPool("Status");
            buffList = new List<StatusIconManager>();
            debuffList = new List<StatusIconManager>();
        }

        private void OnEnable()
        {
            battleBehaviour.StatusManager.OnStatusApplied += OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus += OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged += OnShieldsChanged;
            battleBehaviour.StatusManager.OnBuffsRemoved += OnRemoveBuffs;
            battleBehaviour.StatusManager.OnDebuffsRemoved += OnRemoveDebuffs;
            battleBehaviour.StatusManager.OnSingletonStatusRemoved += OnRemoveSingleton;
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.OnStatusApplied -= OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus -= OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged -= OnShieldsChanged;
            battleBehaviour.StatusManager.OnBuffsRemoved -= OnRemoveBuffs;
            battleBehaviour.StatusManager.OnDebuffsRemoved -= OnRemoveDebuffs;
            battleBehaviour.StatusManager.OnSingletonStatusRemoved -= OnRemoveSingleton;
        }

        private void OnStatusApplied(BattleStatusManager sender, StatusType statusType, float duration)
        {
            OnStatusApplied(statusType, duration);
        }

        private void OnStatusApplied(StatusType statusType, float duration)
        {
            if ((statusType == StatusType.Shield && haveShields) || (statusType == StatusType.DamageImprovement && haveDamageImprovement))
            {
                return;
            }
            StatusData status = GetStatus(statusType);
            GameObject statusIconObject = statusIndicatorPool.GetInstance(instanceParent: statusIndicatorsHolder);
            statusIconObject.transform.localScale = Vector3.one;
            StatusIconManager sim = statusIconObject.GetComponent<StatusIconManager>();
            sim.InitStatusIcon(statusType, status.StatusFrame, status.StatusSprite, status.StatusFrameColor, duration, battleBehaviour.StatusManager, statusIndicatorPool);
            sim.OnStatusTerminated += OnStatusTerminated;
            if (statusType == StatusType.Shield)
            {
                haveShields = true;
            }

            if (statusType == StatusType.DamageImprovement)
            {
                haveDamageImprovement = true;
            }

            if (statusType == StatusType.Blind || statusType == StatusType.Debuff || statusType == StatusType.DoT || statusType == StatusType.DoTFire || statusType == StatusType.Slow || statusType == StatusType.Stun)
            {
                debuffList.Add(sim);
            } else if (statusType == StatusType.Buff || statusType == StatusType.DamageImprovement || statusType == StatusType.Speed || statusType == StatusType.ParryPrepared || statusType == StatusType.ShieldPrepared)
            {
                buffList.Add(sim);
            }
        }

        private void OnShieldsChanged(CharacterHealth sender, int delta, int total, bool isDamage)
        {
            if (total > 0)
            {
                OnStatusApplied(StatusType.Shield, -1f);
            }
        }

        private void OnResetStatus(BattleStatusManager sender)
        {
            haveShields = false;
            haveDamageImprovement = false;
        }

        private StatusData GetStatus(StatusType statusType)
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

        private void OnStatusTerminated(StatusIconManager sender, StatusType type)
        {
            if (type == StatusType.Shield)
            {
                haveShields = false;
            }

            if (type == StatusType.DamageImprovement)
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

        private void OnRemoveSingleton(BattleStatusManager statusManager, StatusType statusType)
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
        }
    }
}