using Laresistance.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class RegenerationStatusEffect : StatusEffect
    {
        public class HealOverTime
        {
            public int power;
            public float timer;
            public int ticked;
        }

        private List<HealOverTime> healsOverTime;
        private int currentTotalHeal = 0;

        public RegenerationStatusEffect(BattleStatusManager statusManager) : base (statusManager)
        {
            healsOverTime = new List<HealOverTime>();
        }

        public override StatusType StatusType => StatusType.Regeneration;

        public override bool IsGoodStatus(float value)
        {
            return true;
        }

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.healOverTimeDuration.GetValue();
            GetDuration(ref duration, IsGoodStatus(value));
            healsOverTime.Add(new HealOverTime() { power = (int)value, ticked = 0, timer = 0f });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Regeneration, duration);
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var healOverTime in healsOverTime)
            {
                other.AddValue(sourceStatusManager, healOverTime.power);
            }
        }

        public override float GetValue()
        {
            float temp = currentTotalHeal;
            currentTotalHeal = 0;
            return temp;
        }

        public override void RemoveBuff()
        {
            healsOverTime.Clear();
        }

        public override void Tick(float delta)
        {
            for (int i = healsOverTime.Count - 1; i >= 0; --i)
            {
                HealOverTime hot = healsOverTime[i];
                hot.timer += delta;
                if (hot.timer >= GameConstantsBehaviour.Instance.healOverTimeTickDelay.GetValue())
                {
                    currentTotalHeal += hot.power;
                    hot.ticked++;
                    hot.timer = hot.timer - GameConstantsBehaviour.Instance.healOverTimeTickDelay.GetValue();

                    float duration = GameConstantsBehaviour.Instance.healOverTimeDuration.GetValue();
                    GetDuration(ref duration, true);

                    if (hot.ticked >= Mathf.FloorToInt(duration / GameConstantsBehaviour.Instance.healOverTimeTickDelay.GetValue()))
                    {
                        healsOverTime.Remove(hot);
                    }
                }
            }
        }

        public override void RemoveStatus()
        {
            RemoveBuff();
        }
    }
}