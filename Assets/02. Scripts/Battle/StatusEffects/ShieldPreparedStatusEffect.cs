using Laresistance.Behaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Battle
{
    public class ShieldPreparedStatusEffect : StatusEffect
    {
        private float shieldPreparedTimer;
        private UnityAction onShieldStatusFinished;

        private float PreparationTime => GameConstantsBehaviour.Instance.shieldPreparationTime.GetValue();

        public ShieldPreparedStatusEffect(BattleStatusManager statusManager) : base(statusManager) { }
        public override StatusType StatusType => StatusType.ShieldPrepared;

        public override bool IsGoodStatus(float value)
        {
            return true;
        }

        public void SetCallback(UnityAction callback)
        {
            onShieldStatusFinished = callback;
        }

        protected override void AddValue(float value)
        {
            shieldPreparedTimer = PreparationTime;
            GetDuration(ref shieldPreparedTimer, IsGoodStatus(value));
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.ShieldPrepared, shieldPreparedTimer);
        }

        public override float GetValue()
        {
            return shieldPreparedTimer;
        }

        public override void Tick(float delta)
        {
            if (shieldPreparedTimer > 0f)
            {
                shieldPreparedTimer -= delta;
                shieldPreparedTimer = Mathf.Max(shieldPreparedTimer, 0f);
                if (shieldPreparedTimer == 0f)
                {
                    onShieldStatusFinished?.Invoke();
                }
            }
        }

        public override void RemoveBuff()
        {
            shieldPreparedTimer = 0f;
        }

        public override bool HaveBuff()
        {
            return shieldPreparedTimer > 0f;
        }

        public override bool AppliesBuff()
        {
            return shieldPreparedTimer > 0f;
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