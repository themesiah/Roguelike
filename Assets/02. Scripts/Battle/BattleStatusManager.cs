using Laresistance.Core;
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
        public static float DAMAGE_OVER_TIME_DURATION = 9f;
        public static float DAMAGE_OVER_TIME_TICK_DELAY = 1.5f;
        public static float STARTING_ENERGY = 3f;
        public static float MAX_ENERGY = 10f;
        #endregion

        #region Private Variables
        private List<SpeedEffect> speedModifiers;
        private List<DamageOverTime> damageOverTimes;
        private List<DamageImprovement> damageImprovements;
        private List<TempDamageChange> tempDamageModifications;
        private EquipmentEvents equipmentEvents;
        private bool stunned = false;
        private float stunTimer;
        private float energyPerSecond;
        // Damage, heal and shield modifiers
        #endregion

        #region Public variables
        public CharacterHealth health { get; private set; }
        public float CurrentEnergy { get; private set; }
        public int UsableEnergy { get { return Mathf.FloorToInt(CurrentEnergy); } }
        public Transform TargetPivot { get; private set; }
        #endregion

        #region Events
        public delegate void OnSpeedModifierAppliedHandler(BattleStatusManager sender, float coeficient, float currentSpeedModifier);
        public event OnSpeedModifierAppliedHandler OnSpeedModifierApplied;
        public delegate void OnDamageOverTimeAppliedHandler(BattleStatusManager sender, int power);
        public event OnDamageOverTimeAppliedHandler OnDamageOverTimeApplied;
        public delegate void OnDamageImprovementAppliedHandler(BattleStatusManager sender, float coeficient, float currentDamageImprovement);
        public event OnDamageImprovementAppliedHandler OnDamageImprovementApplied;
        public delegate void OnStunHandler(BattleStatusManager sender, float stunPercent);
        public event OnStunHandler OnStun;
        public delegate void OnEnergyChangedHandler(float currentEnergy, int usableEnergy);
        public event OnEnergyChangedHandler OnEnergyChanged;
        #endregion

        #region Public methods
        public BattleStatusManager(CharacterHealth health, Transform targetPivot = null, float energyPerSecond = 1f)
        {
            this.health = health;
            speedModifiers = new List<SpeedEffect>();
            damageOverTimes = new List<DamageOverTime>();
            damageImprovements = new List<DamageImprovement>();
            tempDamageModifications = new List<TempDamageChange>();
            this.energyPerSecond = energyPerSecond;
            this.TargetPivot = targetPivot;
        }

        public void ProcessStatus(float delta, float energySpeedModifier)
        {
            int totalDamage = 0;
            for(int i = damageOverTimes.Count-1; i >= 0; --i)
            {
                DamageOverTime dot = damageOverTimes[i];
                dot.timer += delta * energySpeedModifier;
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
                tdm.timer += delta * energySpeedModifier;
                if (tdm.timer >= DAMAGE_MODIFIER_DURATION)
                {
                    tempDamageModifications.Remove(tdm);
                }
            }
            for (int i = speedModifiers.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedModifiers[i];
                se.timer += delta * energySpeedModifier;
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
            if (!BattleAbilityManager.Executing || BattleAbilityManager.executingBasicSkill)
            {
                if (stunned)
                {
                    stunTimer -= delta * energySpeedModifier;
                    if (stunTimer <= 0f)
                    {
                        stunned = false;
                    }
                }
                else
                {
                    float currentProduction = energyPerSecond;
                    equipmentEvents?.OnGetEnergyProduction?.Invoke(ref currentProduction);
                    CurrentEnergy = Mathf.Min(MAX_ENERGY, CurrentEnergy + currentProduction * delta * GetSpeedModifier() * energySpeedModifier);
                    OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
                }
            }
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

        public void ResetStatus()
        {
            damageImprovements.Clear();
            speedModifiers.Clear();
            damageOverTimes.Clear();
            tempDamageModifications.Clear();
            stunned = false;
            float startingEnergy = STARTING_ENERGY;
            equipmentEvents?.OnGetStartingEnergy?.Invoke(ref startingEnergy);
            CurrentEnergy = startingEnergy;
        }

        public void Stun(float time)
        {
            OnStun?.Invoke(this, time);
            stunned = true;
            stunTimer = time;
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
            stunned = false;
        }

        public void SetEquipmentEvents(EquipmentEvents equipmentEvents)
        {
            this.equipmentEvents = equipmentEvents;
        }

        public EquipmentEvents GetEquipmentEvents()
        {
            return this.equipmentEvents;
        }

        public void AddEnergy(float energy)
        {
            float energyToWin = energy * GetSpeedModifier();
            equipmentEvents?.OnGetEnergyProduction?.Invoke(ref energyToWin);
            CurrentEnergy = Mathf.Min(MAX_ENERGY, CurrentEnergy + energyToWin);
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
        }

        public bool ConsumeEnergy(int energy)
        {
            if (!CanExecute(energy))
            {
                return false;
            }
            CurrentEnergy -= (float)energy;
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
            return true;
        }

        public bool CanExecute(int energy)
        {
            return UsableEnergy >= energy && !stunned;
        }

        public void RemoveEnergy(float energy)
        {
            CurrentEnergy = Mathf.Max(0f, CurrentEnergy + energy);
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
        }
        #endregion
    }
}