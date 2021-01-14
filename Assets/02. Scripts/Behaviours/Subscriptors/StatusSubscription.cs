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

        private void Start()
        {
            statusIndicatorPool = PoolInitializerBehaviour.GetPool("Status");
        }

        private void OnEnable()
        {
            battleBehaviour.StatusManager.OnStatusApplied += OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus += OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged += OnShieldsChanged;
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.OnStatusApplied -= OnStatusApplied;
            battleBehaviour.StatusManager.OnResetStatus -= OnResetStatus;
            battleBehaviour.StatusManager.health.OnShieldsChanged -= OnShieldsChanged;
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
        }
    }
}