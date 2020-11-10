﻿using Laresistance.Core;
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
            public float timer;
        }

        public class DamageOverTime
        {
            public int power;
            public float timer;
            public int ticked;
        }

        public class DamageImprovement // Infinite
        {
            public float coeficient;
        }

        public class TempDamageChange
        {
            public float modifier;
            public float timer;
        }
        #endregion

        #region Constants
        public static float SPEED_MODIFIER_DURATION = 3f;
        public static float DAMAGE_MODIFIER_DURATION = 3f;
        public static float DAMAGE_OVER_TIME_DURATION = 3f;
        public static float DAMAGE_OVER_TIME_TICK_DELAY = 0.5f;
        #endregion

        #region Private Variables
        private List<SpeedEffect> speedModifiers;
        private List<DamageOverTime> damageOverTimes;
        private List<DamageImprovement> damageImprovements;
        private List<TempDamageChange> tempDamageModifications;
        private EquipmentEvents equipmentEvents;
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
        public delegate void OnDamageImprovementAppliedHandler(BattleStatusManager sender, float coeficient, float currentDamageImprovement);
        public event OnDamageImprovementAppliedHandler OnDamageImprovementApplied;
        public delegate void OnStunHandler(BattleStatusManager sender);
        public event OnStunHandler OnStun;
        public delegate void OnCooldownsAdvanceHandler(BattleStatusManager sender);
        public event OnCooldownsAdvanceHandler OnCooldownsAdvance;
        #endregion

        #region Public methods
        public BattleStatusManager(CharacterHealth health)
        {
            this.health = health;
            speedModifiers = new List<SpeedEffect>();
            damageOverTimes = new List<DamageOverTime>();
            damageImprovements = new List<DamageImprovement>();
            tempDamageModifications = new List<TempDamageChange>();
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
            for (int i = tempDamageModifications.Count - 1; i >= 0; --i)
            {
                TempDamageChange tdm = tempDamageModifications[i];
                tdm.timer += delta;
                if (tdm.timer >= DAMAGE_MODIFIER_DURATION)
                {
                    tempDamageModifications.Remove(tdm);
                }
            }
            for (int i = speedModifiers.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedModifiers[i];
                se.timer += delta;
                if (se.timer >= SPEED_MODIFIER_DURATION)
                {
                    speedModifiers.Remove(se);
                }
            }
            if (totalDamage > 0)
            {
                health.TakeDamage(totalDamage, null);
            }
            health.Tick(delta);
        }

        public void ApplySpeedModifier(float coeficient)
        {
            speedModifiers.Add(new SpeedEffect() { speedCoeficient = coeficient, timer = 0f });
            OnSpeedModifierApplied?.Invoke(this, coeficient, GetSpeedModifier());
        }

        public void ApplyDamageOverTime(int power)
        {
            damageOverTimes.Add(new DamageOverTime() { power = power, timer = 0, ticked = 0 });
            OnDamageOverTimeApplied?.Invoke(this, power);
        }

        public void ApplyDamageImprovement(float coeficient)
        {
            damageImprovements.Add(new DamageImprovement() { coeficient = coeficient });
            OnDamageImprovementApplied?.Invoke(this, coeficient, GetDamageModifier());
        }

        public void ApplyTempDamageModification(float coeficient)
        {
            tempDamageModifications.Add(new TempDamageChange() { modifier = coeficient, timer = 0f });
            OnDamageImprovementApplied?.Invoke(this, coeficient, GetDamageModifier());
        }

        public float GetSpeedModifier()
        {
            float speedModifier = 1f;
            for (int i = speedModifiers.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedModifiers[i];
                speedModifier *= se.speedCoeficient;
            }
            return speedModifier;
        }

        public float GetDamageModifier()
        {
            float damageModifier = 1f;
            foreach(var modifier in damageImprovements)
            {
                damageModifier *= modifier.coeficient;
            }
            float totalModifier = 0f;
            for (int i = tempDamageModifications.Count -1; i >= 0; --i)
            {
                var modifier = tempDamageModifications[i];
                totalModifier += modifier.modifier;
            }

            // Total modifier adds the damage after damage improvement, which is infinite and "special"
            damageModifier += totalModifier;

            return damageModifier;
        }

        public void ResetModifiers()
        {
            damageImprovements.Clear();
            speedModifiers.Clear();
            damageOverTimes.Clear();
            tempDamageModifications.Clear();
        }

        public void Stun()
        {
            OnStun?.Invoke(this);
        }

        public void Cure()
        {
            damageOverTimes.Clear();
            for(int i = tempDamageModifications.Count-1; i >= 0; --i)
            {
                if (tempDamageModifications[i].modifier < 0f)
                {
                    tempDamageModifications.RemoveAt(i);
                }
            }
            for(int i = speedModifiers.Count-1; i >= 0; --i)
            {
                if (speedModifiers[i].speedCoeficient < 1f)
                {
                    speedModifiers.RemoveAt(i);
                }
            }
        }

        public void AdvanceCooldowns()
        {
            OnCooldownsAdvance?.Invoke(this);
        }

        public void SetEquipmentEvents(EquipmentEvents equipmentEvents)
        {
            this.equipmentEvents = equipmentEvents;
        }

        public EquipmentEvents GetEquipmentEvents()
        {
            return this.equipmentEvents;
        }
        #endregion
    }
}