﻿using Laresistance.Behaviours;
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

        public override void AddValue(float value)
        {
            damageOverTimes.Add(new DamageOverTime() { power = (int)value, timer = 0, ticked = 0 });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.DoT, GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue());
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
                    if (dot.ticked >= Mathf.CeilToInt(GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue() / GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue()))
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
    }
}