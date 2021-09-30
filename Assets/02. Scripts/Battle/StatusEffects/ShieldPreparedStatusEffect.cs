using Laresistance.Behaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Battle
{
    public class ShieldPreparedStatusEffect : StatusEffect
    {
        private static float GRACE_TIME = 0.5f; // THIS WILL BE WHEN PARRY PREPARATION ANIMATION IS FINISHED

        private float shieldPreparedTimer;
        private UnityAction onShieldStatusFinished;

        private float PreparationTime => GameConstantsBehaviour.Instance.shieldPreparationTime.GetValue();

        public ShieldPreparedStatusEffect(BattleStatusManager statusManager) : base(statusManager) { }
        public override StatusType StatusType => StatusType.ShieldPrepared;

        public void SetCallback(UnityAction callback)
        {
            onShieldStatusFinished = callback;
        }

        public override void AddValue(float value)
        {
            shieldPreparedTimer = PreparationTime;
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
            return shieldPreparedTimer > 0f && shieldPreparedTimer < (PreparationTime - GRACE_TIME);
        }

        public override void CopyTo(StatusEffect other)
        {
        }
    }
}