using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Battle
{
    public class BattleStatusManager
    {

        #region Private Variables
        private EquipmentsContainer equipmentsContainer;
        private Dictionary<StatusType, StatusEffect> statusEffects;
        private List<StatusEffect> statusEffectsList;
        private float energyPerSecond;
        #endregion

        #region Public variables
        public CharacterHealth health { get; private set; }
        public float CurrentEnergy { get; private set; }
        public int UsableEnergy { get { return Mathf.FloorToInt(CurrentEnergy); } }
        public Transform TargetPivot { get; private set; }
        public BattleAbility NextAbility { get; private set; }
        public bool IsBossType { get; private set; }

        public bool Stunned => GetStatus(StatusType.Stun).HaveDebuff();
        public bool WillParry => GetStatus(StatusType.ParryPrepared).AppliesBuff();
        public bool WillBlock => GetStatus(StatusType.ShieldPrepared).AppliesBuff();
        public bool HaveParry => GetStatus(StatusType.ParryPrepared).HaveBuff();
        public bool HaveBlock => GetStatus(StatusType.ShieldPrepared).HaveBuff();
        #endregion

        #region Events
        public delegate void OnStatusAppliedHandler(BattleStatusManager sender, StatusIconType statusType, float duration);
        public OnStatusAppliedHandler OnStatusApplied;
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
        public event OnTickHandler OnRealTimeTick;
        public delegate void OnResetStatusHandler(BattleStatusManager sender);
        public event OnResetStatusHandler OnResetStatus;
        public delegate void OnStatusesRemovedHandler(BattleStatusManager sender);
        public event OnStatusesRemovedHandler OnBuffsRemoved;
        public event OnStatusesRemovedHandler OnDebuffsRemoved;
        public delegate void OnSingletonStatusRemovedHandler(BattleStatusManager sender, StatusIconType statusType);
        public OnSingletonStatusRemovedHandler OnSingletonStatusRemoved;
        public delegate void OnSetBattleManager(CharacterBattleManager cbm);
        public event OnSetBattleManager SetBattleManager;
        public delegate void OnSupportAbilityExecutedHandler(BattleStatusManager sender);
        public event OnSupportAbilityExecutedHandler OnSupportAbilityExecuted;
        #endregion

        #region Public methods
        public BattleStatusManager(CharacterHealth health, Transform targetPivot = null, float energyPerSecond = 1f)
        {
            equipmentsContainer = new EquipmentsContainer();
            this.health = health;
            statusEffectsList = new List<StatusEffect>();
            statusEffects = new Dictionary<StatusType, StatusEffect>();
            this.energyPerSecond = energyPerSecond;
            this.TargetPivot = targetPivot;
            health.OnDeath += OnDeath;
        }

        private void OnDeath(CharacterHealth sender)
        {
            ResetStatus();
        }

        public void ProcessStatus(float delta, float speedModifier)
        {
            foreach(var statusEffect in statusEffectsList)
            {
                statusEffect.RealtimeTick(delta);
            }
            OnRealTimeTick?.Invoke(this, delta);
            if (!BattleAbilityManager.Instance.Executing)
            {
                float totalDelta = delta * speedModifier;
                foreach (var statusEffect in statusEffectsList)
                {
                    statusEffect.Tick(totalDelta);
                }

                int totalDamage = (int)GetValueModifier(StatusType.DoT);
                if (totalDamage > 0) {
                    totalDamage = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.DotDamageReceived, totalDamage);
                    health.TakeDotDamage(totalDamage);
                }

                int totalHeal = (int)GetValueModifier(StatusType.Regeneration);
                if (totalHeal > 0) {
                    health.Heal(totalHeal);
                }
                
                health.Tick(delta);
            
                if (!Stunned)
                {
                    float currentProduction = energyPerSecond;
                    currentProduction = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.EnergyProduction, currentProduction);
                    CurrentEnergy = Mathf.Min(GameConstantsBehaviour.Instance.maxEnergy.GetValue(), CurrentEnergy + currentProduction * delta * GetValueModifier(StatusType.Speed) * speedModifier);
                    OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
                }
                OnTick?.Invoke(this, delta);
            }
        }

        #region Status getters and setters

        public StatusEffect GetStatus(StatusType type)
        {
            if (!statusEffects.ContainsKey(type))
            {
                StatusEffect se = StatusEffectFactory.GetStatusEffect(type, this);
                statusEffects.Add(type, se);
                statusEffectsList.Add(se);
            }
            return statusEffects[type];
        }

        public List<StatusEffect> GetAllStatus()
        {
            return statusEffectsList;
        }

        public void ApplyStatusEffect(StatusType type, float value)
        {
            GetStatus(type).AddValue(value);
            if (((RushStatusEffect)GetStatus(StatusType.Rush)).HaveRush())
            {
                GetStatus(type).Cure();
            }
        }

        public float GetValueModifier(StatusType statusType)
        {
            return GetStatus(statusType).GetValue();
        }

        public void BattleStart()
        {
            ResetStatus();
            health.BattleStart();
        }

        public void ResetStatus()
        {
            foreach(var statusEffect in statusEffectsList)
            {
                statusEffect.RemoveStatus();
            }
            OnResetStatus?.Invoke(this);
        }

        public void PrepareParry()
        {
            GetStatus(StatusType.ParryPrepared).AddValue(0f);
        }

        public void PrepareShield()
        {
            GetStatus(StatusType.ShieldPrepared).AddValue(0f);
        }

        public void ParryExecuted()
        {
            Debug.LogWarning("Parry Executed");
            GetStatus(StatusType.ParryPrepared).RemoveBuff();
            OnSingletonStatusRemoved?.Invoke(this, StatusIconType.ParryPrepared);
        }

        public void BlockExecuted()
        {
            Debug.LogWarning("Block Executed");
            GetStatus(StatusType.ShieldPrepared).RemoveBuff();
            OnSingletonStatusRemoved?.Invoke(this, StatusIconType.ShieldPrepared);
        }

        public void Cure()
        {
            foreach(var statusEffect in statusEffectsList)
            {
                statusEffect.Cure();
            }
            OnDebuffsRemoved?.Invoke(this);
        }

        public void RemoveBuffs()
        {
            foreach (var statusEffect in statusEffectsList)
            {
                statusEffect.RemoveBuff();
            }
            OnBuffsRemoved?.Invoke(this);
        }

        public bool HaveDebuff()
        {
            foreach(var statusEffect in statusEffectsList)
            {
                if (statusEffect.HaveDebuff())
                    return true;
            }
            return false;
        }

        public bool HaveBuff()
        {
            foreach (var statusEffect in statusEffectsList)
            {
                if (statusEffect.HaveBuff())
                    return true;
            }
            return false;
        }
        #endregion

        public void BattleEnd()
        {
            health.RemoveShields();
            CurrentEnergy = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.AfterBattleEnergyLoss, CurrentEnergy);
            int healAmount = 0;
            healAmount = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.BattleEndHeal, healAmount);
            if (healAmount > 0)
                health.Heal(healAmount);
        }

        public void ExecuteSupportAbility()
        {
            OnSupportAbilityExecuted?.Invoke(this);
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
            float energyToWin = equipmentsContainer.ModifyValue(Equipments.EquipmentSituation.EnergyProduction, energy);
            CurrentEnergy = Mathf.Min(GameConstantsBehaviour.Instance.maxEnergy.GetValue(), CurrentEnergy + energyToWin);
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
        }

        public bool ConsumeEnergy(int energy, bool canBeUsedStunned)
        {
            if (!CanExecute(energy, canBeUsedStunned))
            {
                return false;
            }
            CurrentEnergy -= (float)energy;
            OnEnergyChanged?.Invoke(CurrentEnergy, UsableEnergy);
            return true;
        }

        public bool CanExecute(int energy, bool canBeUsedStunned)
        {
            return UsableEnergy >= energy && (!Stunned || canBeUsedStunned);
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

        public void SetCharacterBattleManager(CharacterBattleManager cbm)
        {
            SetBattleManager?.Invoke(cbm);
        }

        public void SetBossType(bool bossType)
        {
            IsBossType = bossType;
        }
        #endregion
    }
}