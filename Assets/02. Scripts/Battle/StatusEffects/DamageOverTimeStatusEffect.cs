using Laresistance.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class DamageOverTimeStatusEffect : StatusEffect
    {
        public class DamageOverTime
        {
            public int power;
            public float timer;
            public int ticked;
        }

        private List<DamageOverTime> damageOverTimes;
        private int currentTotalDamage = 0;

        public DamageOverTimeStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            damageOverTimes = new List<DamageOverTime>();
        }

        public override StatusType StatusType => StatusType.DoT;

        public override bool IsGoodStatus(float value)
        {
            return false;
        }

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue();
            GetDuration(ref duration, IsGoodStatus(value));
            damageOverTimes.Add(new DamageOverTime() { power = (int)value, timer = 0, ticked = 0 });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.DoT, duration);
        }

        public override float GetValue()
        {
            float temp = currentTotalDamage;
            currentTotalDamage = 0;
            return temp;
        }

        public override void Tick(float delta)
        {
            for (int i = damageOverTimes.Count - 1; i >= 0; --i)
            {
                DamageOverTime dot = damageOverTimes[i];
                dot.timer += delta;
                if (dot.timer >= GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue())
                {
                    currentTotalDamage += dot.power;
                    dot.ticked++;
                    dot.timer = dot.timer - GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue();

                    float duration = GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue();
                    GetDuration(ref duration, false);

                    if (dot.ticked >= Mathf.FloorToInt(duration / GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue()))
                    {
                        damageOverTimes.Remove(dot);
                    }
                }
            }
        }

        public override void Cure()
        {
            currentTotalDamage = 0;
            damageOverTimes.Clear();
        }

        public override bool HaveDebuff()
        {
            return damageOverTimes.Count > 0;
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var dot in damageOverTimes)
            {
                other.AddValue(sourceStatusManager, dot.power);
            }
        }

        public override void RemoveStatus()
        {
            Cure();
        }
    }
}