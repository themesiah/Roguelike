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

        public override void AddValue(float value)
        {
            healsOverTime.Add(new HealOverTime() { power = (int)value, ticked = 0, timer = 0f });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Regeneration, GameConstantsBehaviour.Instance.healOverTimeDuration.GetValue());
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var healOverTime in healsOverTime)
            {
                other.AddValue(healOverTime.power);
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
                    if (hot.ticked >= Mathf.CeilToInt(GameConstantsBehaviour.Instance.healOverTimeDuration.GetValue() / GameConstantsBehaviour.Instance.healOverTimeTickDelay.GetValue()))
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