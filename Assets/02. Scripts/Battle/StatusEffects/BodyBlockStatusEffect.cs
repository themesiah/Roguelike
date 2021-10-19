using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BodyBlockStatusEffect : StatusEffect
    {
        private float blockValue = 0f;

        public override StatusType StatusType => StatusType.BodyBlockStatus;

        public BodyBlockStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {

        }

        public override void AddValue(float value)
        {
            blockValue = value;
            statusManager.health.SetPercentDamageBlock(blockValue);
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.BodyDamageBlock, -1f);
        }

        public override void CopyTo(StatusEffect other)
        {
            other.AddValue(blockValue);
        }

        public override float GetValue()
        {
            return blockValue;
        }

        public override void Tick(float delta)
        {
        }

        public override bool HaveBuff()
        {
            return false;
        }

        public bool HaveBlock()
        {
            return blockValue > 0f;
        }

        public override void RemoveBuff()
        {
            
        }

        public void CancelBlock()
        {
            blockValue = 0f;
            statusManager.health.SetPercentDamageBlock(blockValue);
            statusManager.OnSingletonStatusRemoved?.Invoke(statusManager, StatusIconType.BodyDamageBlock);
        }
    }
}