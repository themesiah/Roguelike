using System.Collections.Generic;
using System.Collections;
using Laresistance.Core;
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;
using Laresistance.Data;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif

namespace Laresistance.Battle
{
    public class BattleAbility : ShowableElement
    {
        public enum AbilityState
        {
            Idle = 0,
            WaitingInQueue,
            Executing,
            Finished
        }
        private static int MAX_EFFECTS = 200;
        private int energyCost;
        private List<BattleEffect> effects = default;
        private EquipmentsContainer equipmentsContainer;
        private BattleStatusManager statusManager;
        private bool executingAbility = false;
        private float cooldownTimer = 0f;
        private float internalCooldown = 0f; // Only for enemies, for non standard skills with special AISpecification
        private AbilityState abilityState;
        public AbilityData data { get; private set; }
        public int Weight { get; private set; }
        public float Cooldown { get; private set; }
        public float InternalCooldown { get; private set; }
        public float TimerProgress => cooldownTimer / Cooldown;
        public bool IsShieldAbility { get; private set; }
        public bool IsOffensiveAbility { get; private set; }
        public Sprite AbilityIcon { get; private set; }
        public Sprite AbilityFrame { get; private set; }
        public Minion parentMinion { get; private set; }
        public Player parentPlayer { get; private set; }
        public AbilityState State => abilityState;
        public int AbilityLevel { 
            get 
            { 
                if (parentMinion == null && parentPlayer == null)
                {
                    return 1;
                } else if (parentMinion != null)
                {
                    return parentMinion.Level;
                } else
                {
                    return parentPlayer.Level;
                }
            }
        }

        public int CurrentPlayerSlot { get; set; }

        public BattleAbility(List<BattleEffect> effectsToSet, int energyCost, int weight, float cooldown, BattleStatusManager statusManager, EquipmentsContainer equipments, Sprite icon = null, AbilityData data = null)
        {
            if (effectsToSet == null || effectsToSet.Count == 0)
                throw new System.Exception("Abilities should have at least one effect");
            if (effectsToSet.Count > MAX_EFFECTS)
                throw new System.Exception("Abilities can only have up to " + MAX_EFFECTS + " effects");
            effects = effectsToSet;
            foreach(var effect in effects)
            {
                effect.SetParentAbility(this);
            }
            this.equipmentsContainer = equipments;
            this.statusManager = statusManager;
            this.energyCost = energyCost;
            this.Weight = weight;
            this.Cooldown = cooldown;
            if (data != null)
            {
                this.InternalCooldown = data.InternalCooldown;
                this.internalCooldown = data.InternalCooldown;
            }
            this.AbilityIcon = icon;
            this.AbilityFrame = data.FrameGraphic;
            this.data = data;
        }

        public BattleAbility Copy()
        {
            var ability = new BattleAbility(effects, energyCost, Weight, Cooldown, statusManager, equipmentsContainer, AbilityIcon, data);
            if (IsShieldAbility)
            {
                ability.SetShieldAbility();
            }
            if (IsOffensiveAbility)
            {
                ability.SetOffensiveAbility();
            }
            ability.CurrentPlayerSlot = CurrentPlayerSlot;

            return ability;
        }

        public void SetAbilityState(AbilityState state)
        {
            abilityState = state;
        }

        public void SetShieldAbility()
        {
            IsShieldAbility = true;
        }

        public void SetOffensiveAbility()
        {
            IsOffensiveAbility = true;
        }

        public void SetEquipmentsContainer(EquipmentsContainer equipments)
        {
            this.equipmentsContainer = equipments;
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            foreach(var effect in effects)
            {
                effect.SetStatusManager(selfStatus);
            }
            var oldStatus = this.statusManager;
            this.statusManager = selfStatus;
        }

        public BattleStatusManager GetStatusManager()
        {
            return this.statusManager;
        }

        public int GetEffectPower(int index, int level)
        {
            if (index > effects.Count - 1 || index < 0)
                throw new System.Exception("Invalid index for effect power");
            return effects[index].GetPower(level, equipmentsContainer);
        }

        public string GetAbilityText(int level)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var effect in effects)
            {
                builder.Append(effect.GetEffectString(level, equipmentsContainer));
                builder.Append(" ");
            }
            if (GetCost() > 0)
                builder.Append(Texts.GetText("ABILITY_COOLDOWN", GetCost()));
            return builder.ToString();
        }

        public string GetShortAbilityText(int level)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Texts.GetText(data.ShortDesc));
            builder.Append(" (");
            for (int i = 0; i < effects.Count; ++i)
            {
                var effect = effects[i];
                builder.Append(effect.GetShortEffectString(level, equipmentsContainer));
                if (i < effects.Count - 1)
                {
                    builder.Append(",");
                }
            }
            builder.Append(")");

            return builder.ToString();
        }

        public string GetSimpleAbilityText()
        {
            return Texts.GetText(data.ShortDesc);
        }

        public string GetAbilityPowerText(int level)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < effects.Count; ++i)
            {
                var effect = effects[i];
                builder.Append(effect.GetShortEffectString(level, equipmentsContainer));
                if (i < effects.Count - 1)
                {
                    builder.Append("/");
                }
            }
            return builder.ToString();
        }

        public void Tick(float deltaTime)
        {
            if (!BattleAbilityManager.Instance.Executing && cooldownTimer > 0f)
            {
                cooldownTimer -= deltaTime * statusManager.GetValueModifier(StatusType.Speed);
            }
        }

        public void TickInternalCooldown(float deltaTime)
        {
            if (!BattleAbilityManager.Instance.Executing && internalCooldown > 0f)
            {
                internalCooldown -= deltaTime * statusManager.GetValueModifier(StatusType.Speed);
            }
        }

        public void ForceTick(float deltaTime, bool modifyValue)
        {
            float totalDelta = deltaTime;
            if (modifyValue)
            {
                totalDelta = totalDelta * statusManager.GetValueModifier(StatusType.Speed);
            }

            cooldownTimer -= totalDelta;
            internalCooldown -= totalDelta;
        }

        public IEnumerator ExecuteAbility(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, ScriptableIntReference bloodRef = null)
        {
            // We need to actually start a new coroutine here because prioritary abilities can and should be processed BEFORE non prioritary abilities.
            // If we are in unity editor and are NOT playing, we are using an editor simulation. In that case we need EditorCoroutineUtility.
            // If we are not in the editor (a build) or we are playing (play mode or unity tests) we use the coroutine helper behaviour.
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(ExecuteAbilityCoroutine(allies, targets, level, animator, bloodRef));
            }
            else
            {
#endif
                CoroutineHelperBehaviour.GetInstance().StartCoroutine(ExecuteAbilityCoroutine(allies, targets, level, animator, bloodRef));
#if UNITY_EDITOR
            }
#endif
                yield return null;
        }

        public IEnumerator ExecuteAbilityCoroutine(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, ScriptableIntReference bloodRef = null)
        {
            if (CanBeExecuted())
            {
                executingAbility = true;
                allies[0].health.OnDeath += CancelExecution;
                yield return BattleAbilityManager.Instance.ExecuteAbility(this, allies, targets, level, animator, GetAnimationTrigger(), bloodRef);
                allies[0].health.OnDeath -= CancelExecution;
                executingAbility = false;
            }
        }

        public IEnumerator Perform(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, ScriptableIntReference bloodRef = null)
        {
            int expectedSignals = 0;
            int signalsReceived = 0;
            foreach (var effect in effects)
            {
                yield return effect.PerformEffect(allies, targets, level, equipmentsContainer, animator, bloodRef, () => signalsReceived++, (amount) => expectedSignals = amount);
            }
            statusManager.ConsumeEnergy(energyCost, CanBeUsedStunned());
            SetCooldownAsUsed();
            statusManager.AbilityExecuted(this, CurrentPlayerSlot);
            CurrentPlayerSlot = -1;
            while (signalsReceived < expectedSignals)
            {
                yield return null;
            }
        }

        public void CancelByTargetDeath()
        {
            statusManager.CancelAbilityByTargetDeath(this, CurrentPlayerSlot);
        }

        public void SetCooldownAsUsed()
        {
            cooldownTimer = Cooldown;
            internalCooldown = InternalCooldown;
        }

        public void ResetCooldown()
        {
            cooldownTimer = 0f;
            internalCooldown = 0f;
        }

        public void CancelExecution(CharacterHealth sender)
        {
            if (executingAbility)
                BattleAbilityManager.Instance.CancelExecution(this);
        }

        private bool CanBeUsedStunned()
        {
            foreach (var effect in effects)
            {
                if (effect.EffectCanBeUsedStunned())
                    return true;
            }
            return false;
        }

        public bool CanBeUsed()
        {
            if (executingAbility || data.AiSpecification == AbilityDataAISpecification.Advantage)
            {
                return false;
            }
            
            return statusManager.CanExecute(energyCost, CanBeUsedStunned()) && cooldownTimer <= 0f;
        }

        private bool CanBeExecuted()
        {
            if (executingAbility)
            {
                return false;
            }

            return statusManager.CanExecute(energyCost, CanBeUsedStunned()) && cooldownTimer <= 0f;
        }

        public bool CanBeUsedInternalTimer()
        {
            if (executingAbility)
            {
                return false;
            }
            return statusManager.CanExecute(energyCost, CanBeUsedStunned()) && internalCooldown <= 0f;
        }

        public int GetCost()
        {
            return energyCost;
        }

        private string GetAnimationTrigger()
        {
            if (string.IsNullOrEmpty(data.AnimationTriggerOverride))
            {
                return effects[0].GetAnimationTrigger();
            } else
            {
                return data.AnimationTriggerOverride;
            }
        }

        public bool IsPrioritary()
        {
            foreach(var effect in effects)
            {
                if (effect.IsPrioritary)
                    return true;
            }
            return false;
        }

        public void SetParentMinion(Minion m)
        {
            parentMinion = m;
        }

        public void SetParentPlayer(Player p)
        {
            parentPlayer = p;
        }

        public List<BattleStatusManager> GetListOfTargets(BattleStatusManager[] allies, BattleStatusManager[] enemies)
        {
            List<BattleStatusManager> statuses = new List<BattleStatusManager>();
            foreach(var effect in effects)
            {
                var effectStatuses = effect.GetTargets(allies, enemies);
                foreach(var status in effectStatuses)
                {
                    if (!statuses.Contains(status))
                    {
                        statuses.Add(status);
                    }
                }
            }
            return statuses;
        }

        public bool IsInListOfTargets(BattleStatusManager[] allies, BattleStatusManager[] enemies, BattleStatusManager[] statusesToCheck)
        {
            var statuses = GetListOfTargets(allies, enemies);
            foreach(var statusToCheck in statusesToCheck)
            {
                if (statuses.Contains(statusToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInListOfTargets(BattleStatusManager[] allies, BattleStatusManager[] enemies, BattleStatusManager statusToCheck)
        {
            var statuses = GetListOfTargets(allies, enemies);
            return statuses.Contains(statusToCheck);
        }
    }
}