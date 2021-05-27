using Laresistance.Behaviours;
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

        public class BlindStatus
        {
            public float timer;
            public float coeficient;
        }
        #endregion

        #region Private Variables
        private List<SpeedEffect> speedModifiers;
        private List<DamageOverTime> damageOverTimes;
        private List<DamageImprovement> damageImprovements;
        private List<TempDamageChange> tempDamageModifications;
        private List<BlindStatus> blindStatuses;
        private EquipmentsContainer equipmentsContainer;
        public bool Stunned { get; private set; }
        private float stunTimer;
        private float energyPerSecond;
        // Damage, heal and shield modifiers
        #endregion

        #region Public variables
        public CharacterHealth health { get; private set; }
        public float CurrentEnergy { get; private set; }
        public int UsableEnergy { get { return Mathf.FloorToInt(CurrentEnergy); } }
        public Transform TargetPivot { get; private set; }
        public BattleAbility NextAbility { get; private set; }
        #endregion

        #region Events
        public delegate void OnStatusAppliedHandler(BattleStatusManager sender, StatusType statusType, float duration);
        public event OnStatusAppliedHandler OnStatusApplied;
        public delegate void OnEnergyChangedHandler(float currentEnergy, int usableEnergy);
        public event OnEnergyChangedHandler OnEnergyChanged;
        public delegate void OnNextAbilityChangedHandler(BattleAbility nextAbility);
        public event OnNextAbilityChangedHandler OnNextAbilityChanged;
        public delegate void OnAbilityExecutionCancelledByTargetDeathHandler(BattleAbility ability, int slotIndex);
        public event OnAbilityExecutionCancelledByTargetDeathHandler OnAbilityExecutionCancelledByTargetDeath;
        public delegate void OnAbilityExecutedHandler(BattleAbility ability, int slotIndex);
        public event OnAbilityExecutedHandler OnAbilityExecuted;
        public delegate void OnTickHandler(BattleStatusManager sender, float delta);
        public event OnTickHandler OnTick;
        public delegate void OnResetStatusHandler(BattleStatusManager sender);
        public event OnResetStatusHandler OnResetStatus;
        public delegate void OnStatusesRemovedHandler(BattleStatusManager sender);
        public event OnStatusesRemovedHandler OnBuffsRemoved;
        public event OnStatusesRemovedHandler OnDebuffsRemoved;
        #endregion

        #region Public methods
        public BattleStatusManager(CharacterHealth health, Transform targetPivot = null, float energyPerSecond = 1f)
        {
            equipmentsContainer = new EquipmentsContainer();
            this.health = health;
            speedModifiers = new List<SpeedEffect>();
            damageOverTimes = new List<DamageOverTime>();
            damageImprovements = new List<DamageImprovement>();
            tempDamageModifications = new List<TempDamageChange>();
            blindStatuses = new List<BlindStatus>();
            this.energyPerSecond = energyPerSecond;
            this.TargetPivot = targetPivot;
        }

        public void ProcessStatus(float delta, float speedModifier)
        {
            if (!BattleAbilityManager.Instance.Executing || BattleAbilityManager.Instance.executingBasicSkill)
            {
                int totalDamage = 0;
                for(int i = damageOverTimes.Count-1; i >= 0; --i)
                {
                    DamageOverTime dot = damageOverTimes[i];
                    dot.timer += delta * speedModifier;
                    if (dot.timer >= GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue())
                    {
                        totalDamage += dot.power;
                        dot.ticked++;
                        dot.timer = dot.timer - GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue();
                        if (dot.ticked >= Mathf.CeilToInt(GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue() / GameConstantsBehaviour.Instance.damageOverTimeTickDelay.GetValue()))
                        {
                            damageOverTimes.Remove(dot);
                        }
                    }
                }
                totalDamage = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.DotDamageReceived, totalDamage);
                for (int i = tempDamageModifications.Count - 1; i >= 0; --i)
                {
                    TempDamageChange tdm = tempDamageModifications[i];
                    tdm.timer += delta * speedModifier;
                    if (tdm.timer >= GameConstantsBehaviour.Instance.damageModifierDuration.GetValue())
                    {
                        tempDamageModifications.Remove(tdm);
                    }
                }
                for (int i = speedModifiers.Count - 1; i >= 0; --i)
                {
                    SpeedEffect se = speedModifiers[i];
                    se.timer += delta * speedModifier;
                    if (se.timer >= GameConstantsBehaviour.Instance.speedModifierDuration.GetValue())
                    {
                        speedModifiers.Remove(se);
                    }
                }
                for (int i = blindStatuses.Count - 1; i >= 0; --i)
                {
                    BlindStatus bs = blindStatuses[i];
                    bs.timer += delta * speedModifier;
                    if (bs.timer >= GameConstantsBehaviour.Instance.blindDuration.GetValue())
                    {
                        blindStatuses.Remove(bs);
                    }
                }
                if (totalDamage > 0)
                {
                    health.TakeDamage(totalDamage, GetEquipmentsContainer(), new EquipmentsContainer());
                }
                health.Tick(delta);
            
                if (Stunned)
                {
                    stunTimer -= delta * speedModifier;
                    if (stunTimer <= 0f)
                    {
                        Stunned = false;
                    }
                }
                else
                {
                    float currentProduction = energyPerSecond;
                    currentProduction = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.EnergyProduction, currentProduction);
                    CurrentEnergy = Mathf.Min(GameConstantsBehaviour.Instance.maxEnergy.GetValue(), CurrentEnergy + currentProduction * delta * GetSpeedModifier() * speedModifier);
                    OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
                }
                OnTick?.Invoke(this, delta);
            }
        }

        public void ApplySpeedModifier(float coeficient)
        {
            speedModifiers.Add(new SpeedEffect() { speedCoeficient = coeficient, timer = 0f });
            if (coeficient > 1f)
            {
                OnStatusApplied?.Invoke(this, StatusType.Speed, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue());
            } else
            {
                OnStatusApplied?.Invoke(this, StatusType.Slow, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue());
            }
        }

        public void ApplyDamageOverTime(int power)
        {
            damageOverTimes.Add(new DamageOverTime() { power = power, timer = 0, ticked = 0 });
            OnStatusApplied?.Invoke(this, StatusType.DoT, GameConstantsBehaviour.Instance.damageOverTimeDuration.GetValue());
        }

        public void ApplyDamageImprovement(float coeficient)
        {
            damageImprovements.Add(new DamageImprovement() { coeficient = coeficient });
            OnStatusApplied?.Invoke(this, StatusType.DamageImprovement, -1f);
        }

        public void ApplyTempDamageModification(float coeficient)
        {
            tempDamageModifications.Add(new TempDamageChange() { modifier = coeficient, timer = 0f });
            if (coeficient > 1f)
            {
                OnStatusApplied?.Invoke(this, StatusType.Buff, GameConstantsBehaviour.Instance.damageModifierDuration.GetValue());
            } else
            {
                OnStatusApplied?.Invoke(this, StatusType.Debuff, GameConstantsBehaviour.Instance.damageModifierDuration.GetValue());
            }
        }

        public void ApplyBlind(float coeficient)
        {
            blindStatuses.Add(new BlindStatus() { coeficient = coeficient, timer = 0f });
            OnStatusApplied?.Invoke(this, StatusType.Blind, GameConstantsBehaviour.Instance.blindDuration.GetValue());
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

        public float GetHitChance()
        {
            float chance = 1f;
            foreach(BlindStatus bs in blindStatuses)
            {
                chance *= (1f - bs.coeficient);
            }
            return chance;
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
            blindStatuses.Clear();
            Stunned = false;
            OnResetStatus?.Invoke(this);
        }

        public void BattleEnd()
        {
            CurrentEnergy = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.AfterBattleEnergyLoss, CurrentEnergy);
        }

        public void Stun(float time)
        {
            OnStatusApplied?.Invoke(this, StatusType.Stun, time);
            Stunned = true;
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
            blindStatuses.Clear();
            Stunned = false;
            OnDebuffsRemoved?.Invoke(this);
        }

        public void RemoveBuffs()
        {
            for (int i = tempDamageModifications.Count - 1; i >= 0; --i)
            {
                if (tempDamageModifications[i].modifier > 0f)
                {
                    tempDamageModifications.RemoveAt(i);
                }
            }
            for (int i = speedModifiers.Count - 1; i >= 0; --i)
            {
                if (speedModifiers[i].speedCoeficient > 1f)
                {
                    speedModifiers.RemoveAt(i);
                }
            }
            damageImprovements.Clear();
            OnBuffsRemoved?.Invoke(this);
        }

        public void SetEquipmentsContainer(EquipmentsContainer equipments)
        {
            this.equipmentsContainer = equipments;
        }

        public EquipmentsContainer GetEquipmentsContainer()
        {
            return this.equipmentsContainer;
        }

        public void AddEnergy(float energy)
        {
            float energyToWin = energy * GameConstantsBehaviour.Instance.energyPerCardReference.GetValue();
            energyToWin = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.EnergyProduction, energyToWin);
            CurrentEnergy = Mathf.Min(GameConstantsBehaviour.Instance.maxEnergy.GetValue(), CurrentEnergy + energyToWin);
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
            return UsableEnergy >= energy && !Stunned;
        }

        public void RemoveEnergy(float energy)
        {
            CurrentEnergy = Mathf.Max(0f, CurrentEnergy + energy);
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
        }

        public void SetNextAbility(BattleAbility ability)
        {
            NextAbility = ability;
            OnNextAbilityChanged?.Invoke(NextAbility);
        }

        public void CancelAbilityByTargetDeath(BattleAbility ability, int slotIndex)
        {
            OnAbilityExecutionCancelledByTargetDeath?.Invoke(ability, slotIndex);
        }

        public void AbilityExecuted(BattleAbility ability, int slotIndex)
        {
            OnAbilityExecuted?.Invoke(ability, slotIndex);
        }
        #endregion
    }
}