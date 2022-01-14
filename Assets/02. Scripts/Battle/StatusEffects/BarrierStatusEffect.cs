﻿using Laresistance.Behaviours;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class BarrierStatusEffect : StatusEffect
    {
        public class BarrierEffect
        {
            public int barrier;
            public float timer;
        }
        private List<BarrierEffect> barrierEffectInstances;

        public BarrierStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {
            barrierEffectInstances = new List<BarrierEffect>();
        }

        public override StatusType StatusType => StatusType.Barrier;

        public override bool IsGoodStatus(float value)
        {
            return true;
        }

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.barrierBuffDuration.GetValue();
            GetDuration(ref duration, IsGoodStatus(value));
            barrierEffectInstances.Add(new BarrierEffect() { barrier = (int)value, timer = duration });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Barrier, duration);
        }

        public override float GetValue()
        {
            int totalBarrier = 0;
            foreach(var barrierEffect in barrierEffectInstances)
            {
                totalBarrier += barrierEffect.barrier;
            }
            return totalBarrier;
        }

        public override void Tick(float delta)
        {
            for (int i = barrierEffectInstances.Count - 1; i >= 0; --i)
            {
                BarrierEffect se = barrierEffectInstances[i];
                se.timer -= delta;
                if (se.timer <= 0f)
                {
                    barrierEffectInstances.Remove(se);
                }
            }
        }

        public override bool HaveBuff()
        {
            return barrierEffectInstances.Count > 0;
        }

        public override void RemoveBuff()
        {
            barrierEffectInstances.Clear();
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach(var barrier in barrierEffectInstances)
            {
                other.AddValue(sourceStatusManager, barrier.barrier);
            }
        }

        public override void RemoveStatus()
        {
            RemoveBuff();
        }
    }
}