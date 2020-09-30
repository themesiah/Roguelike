using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleStatusManager
    {
        #region Inner Classes
        public class SpeedEffect
        {
            public float speedCoeficient;
            public float setupTime;
        }

        public class DamageOverTime
        {
            public int power;
            public float timer;
            public int ticked;
        }
        #endregion

        #region Constants
        private static float SPEED_MODIFIER_DURATION = 3f;
        private static float DAMAGE_OVER_TIME_DURATION = 3f;
        private static float DAMAGE_OVER_TIME_TICK_DELAY = 0.5f;
        #endregion

        #region Private Variables
        private List<SpeedEffect> speedModifiers;
        private List<DamageOverTime> damageOverTimes;
        // Damage, heal and shield modifiers
        #endregion

        #region Public variables
        public CharacterHealth health { get; private set; }
        #endregion

        #region Events
        public delegate void OnSpeedModifierAppliedHandler(BattleStatusManager sender, float coeficient, float currentSpeedModifier);
        public event OnSpeedModifierAppliedHandler OnSpeedModifierApplied;
        public delegate void OnDamageOverTimeAppliedHandler(BattleStatusManager sender, int power);
        public event OnDamageOverTimeAppliedHandler OnDamageOverTimeApplied;
        #endregion

        #region Public methods
        public BattleStatusManager(CharacterHealth health)
        {
            this.health = health;
            speedModifiers = new List<SpeedEffect>();
            damageOverTimes = new List<DamageOverTime>();
        }

        public void ProcessStatus(float delta)
        {
            int totalDamage = 0;
            for(int i = damageOverTimes.Count-1; i >= 0; --i)
            {
                DamageOverTime dot = damageOverTimes[i];
                dot.timer += delta;
                if (dot.timer >= DAMAGE_OVER_TIME_TICK_DELAY)
                {
                    totalDamage += dot.power;
                    dot.ticked++;
                    dot.timer = dot.timer - DAMAGE_OVER_TIME_TICK_DELAY;
                    if (dot.ticked >= Mathf.CeilToInt(DAMAGE_OVER_TIME_DURATION/DAMAGE_OVER_TIME_TICK_DELAY))
                    {
                        damageOverTimes.Remove(dot);
                    }
                }
            }
            if (totalDamage > 0)
            {
                health.TakeDamage(totalDamage);
            }
        }

        public void ApplySpeedModifier(float coeficient)
        {
            speedModifiers.Add(new SpeedEffect() { speedCoeficient = coeficient, setupTime = Time.time });
            OnSpeedModifierApplied?.Invoke(this, coeficient, GetSpeedModifier());
        }

        public void ApplyDamageOverTime(int power)
        {
            damageOverTimes.Add(new DamageOverTime() { power = power, timer = 0, ticked = 0 });
            OnDamageOverTimeApplied?.Invoke(this, power);
        }

        public float GetSpeedModifier()
        {
            float speedModifier = 1f;
            for (int i = speedModifiers.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedModifiers[i];
                if (Time.time < se.setupTime + SPEED_MODIFIER_DURATION)
                {
                    speedModifier *= se.speedCoeficient;
                }
                else
                {
                    speedModifiers.Remove(se);
                }
            }
            return speedModifier;
        }
        #endregion
    }
}