using Laresistance.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BodyBlockStatusEffect : StatusEffect
    {
        private float blockValue = 0f;
        private float timer = 0f;

        public override StatusType StatusType => StatusType.BodyBlockStatus;

        public BodyBlockStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {

        }

        public override bool IsGoodStatus(float value)
        {
            return true;
        }

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.bodyBlockBuffDuration.GetValue();
            GetDuration(ref duration, IsGoodStatus(value));
            blockValue = value;
            timer = duration;
            statusManager.health.SetPercentDamageBlock(blockValue);
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.BodyDamageBlock, duration);
        }

        public override void CopyTo(StatusEffect other)
        {
            other.AddValue(sourceStatusManager, blockValue);
        }

        public override float GetValue()
        {
            return blockValue;
        }

        public override void Tick(float delta)
        {
            if (timer > 0f)
            {
                timer -= delta;
                if (timer <= 0f)
                {
                    blockValue = 0f;
                }
            }
        }

        public override bool HaveBuff()
        {
            return false;
        }

        public bool HaveBlock()
        {
            return blockValue > 0f;
        }

        public void CancelBlock()
        {
            blockValue = 0f;
            statusManager.health.SetPercentDamageBlock(blockValue);
            statusManager.OnSingletonStatusRemoved?.Invoke(statusManager, StatusIconType.BodyDamageBlock);
        }

        public override void RemoveStatus()
        {
            CancelBlock();
        }
    }
}