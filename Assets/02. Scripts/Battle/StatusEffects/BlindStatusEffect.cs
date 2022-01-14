using Laresistance.Behaviours;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class BlindStatusEffect : StatusEffect
    {
        public class BlindStatus
        {
            public float timer;
            public float coeficient;
        }

        private List<BlindStatus> blindStatuses;

        public BlindStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            blindStatuses = new List<BlindStatus>();
        }

        public override StatusType StatusType => StatusType.Blind;

        public override bool IsGoodStatus(float value)
        {
            return false;
        }

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.blindDuration.GetValue();
            GetDuration(ref duration, IsGoodStatus(value));
            blindStatuses.Add(new BlindStatus() { coeficient = value, timer = duration });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Blind, duration);
        }

        public override float GetValue()
        {
            float chance = 1f;
            foreach (BlindStatus bs in blindStatuses)
            {
                chance *= (1f - bs.coeficient);
            }
            return chance;
        }

        public override void Tick(float delta)
        {
            for (int i = blindStatuses.Count - 1; i >= 0; --i)
            {
                BlindStatus bs = blindStatuses[i];
                bs.timer -= delta;
                if (bs.timer <= 0f)
                {
                    blindStatuses.Remove(bs);
                }
            }
        }

        public override void Cure()
        {
            blindStatuses.Clear();
        }

        public override bool HaveDebuff()
        {
            return blindStatuses.Count > 0;
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var blindEffect in blindStatuses)
            {
                other.AddValue(sourceStatusManager, blindEffect.coeficient);
            }
        }

        public override void RemoveStatus()
        {
            Cure();
        }
    }
}