using Laresistance.Behaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Battle
{
    public class ParryPreparedStatusEffect : StatusEffect
    {
        private float parryPreparedTimer;
        private UnityAction onParryStatusFinished;

        private float PreparationTime => GameConstantsBehaviour.Instance.parryPreparationTime.GetValue();

        public ParryPreparedStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            parryPreparedTimer = 0f;
        }

        public override StatusType StatusType => StatusType.ParryPrepared;

        public void SetCallback(UnityAction callback)
        {
            onParryStatusFinished = callback;
        }

        public override void AddValue(float value)
        {
            parryPreparedTimer = PreparationTime;
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.ParryPrepared, parryPreparedTimer);
        }

        public override float GetValue()
        {
            return parryPreparedTimer;
        }

        public override void Tick(float delta)
        {
            if (parryPreparedTimer > 0f)
            {
                parryPreparedTimer -= delta;
                parryPreparedTimer = Mathf.Max(parryPreparedTimer, 0f);
                if (parryPreparedTimer == 0f)
                    onParryStatusFinished?.Invoke();
            }
        }

        public override void RemoveBuff()
        {
            parryPreparedTimer = -1f;
        }

        public override bool HaveBuff()
        {
            return parryPreparedTimer > 0f;
        }

        public override bool AppliesBuff()
        {
            return parryPreparedTimer > 0f;
        }

        public override void CopyTo(StatusEffect other)
        {
        }

        public override void RemoveStatus()
        {
            RemoveBuff();
        }
    }
}