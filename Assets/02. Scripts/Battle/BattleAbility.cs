using System.Collections.Generic;
using System.Collections;
using Laresistance.Core;
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Diagnostics;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif

namespace Laresistance.Battle
{
    public class BattleAbility
    {
        private static int MAX_EFFECTS = 2;
        private float cooldown;
        private List<BattleEffect> effects = default;
        private float timer = 0f;
        private EquipmentEvents equipmentEvents;
        private BattleStatusManager statusManager;
        private bool executingAbility = false;
        public bool IsShieldAbility { get; private set; }
        public bool IsOffensiveAbility { get; private set; }

        public delegate void OnAbilityTimerChangedHandler(float currentTimer, float cooldown, float percent);
        public event OnAbilityTimerChangedHandler OnAbilityTimerChanged;

        private bool eventsRegistered = false;

        public BattleAbility(List<BattleEffect> effectsToSet, float cooldown, BattleStatusManager statusManager, EquipmentEvents equipmentEvents = null)
        {
            if (effectsToSet == null || effectsToSet.Count == 0)
                throw new System.Exception("Abilities should have at least one effect");
            if (effectsToSet.Count > MAX_EFFECTS)
                throw new System.Exception("Abilities can only have up to " + MAX_EFFECTS + " effects");
            effects = effectsToSet;
            this.cooldown = cooldown;
            this.equipmentEvents = equipmentEvents;
            this.statusManager = statusManager;
            if (statusManager != null)
            {
                RegisterEvents(statusManager, null);
            }
        }

        private void RegisterEvents(BattleStatusManager statusManager, BattleStatusManager oldStatusManager)
        {
            if (eventsRegistered)
            {
                oldStatusManager.OnStun -= Stun;
                oldStatusManager.OnCooldownsAdvance -= AdvanceCooldowns;
            }
            statusManager.OnStun += Stun;
            statusManager.OnCooldownsAdvance += AdvanceCooldowns;
            eventsRegistered = true;
        }

        public void SetShieldAbility()
        {
            IsShieldAbility = true;
        }

        public void SetOffensiveAbility()
        {
            IsOffensiveAbility = true;
        }

        public void SetEquipmentEvents(EquipmentEvents equipmentEvents)
        {
            this.equipmentEvents = equipmentEvents;
        }

        public void SetStatusManager(BattleStatusManager selfStatus)
        {
            foreach(var effect in effects)
            {
                effect.SetStatusManager(selfStatus);
            }
            var oldStatus = this.statusManager;
            this.statusManager = selfStatus;
            RegisterEvents(selfStatus, oldStatus);
        }

        public BattleStatusManager GetStatusManager()
        {
            return this.statusManager;
        }

        public BattleAbility Copy()
        {
            BattleAbility ba = new BattleAbility(effects, GetCooldown(), statusManager, equipmentEvents);
            return ba;
        }

        public int GetEffectPower(int index, int level)
        {
            if (index > effects.Count - 1 || index < 0)
                throw new System.Exception("Invalid index for effect power");
            return effects[index].GetPower(level, equipmentEvents);
        }

        public string GetAbilityText(int level)
        {
            StringBuilder builder = new StringBuilder();
            foreach(var effect in effects)
            {
                builder.Append(effect.GetEffectString(level, equipmentEvents));
                builder.Append(" ");
            }
            if (GetCooldown() > 0f)
                builder.Append(Texts.GetText("ABILITY_COOLDOWN", GetCooldown()));
            return builder.ToString();
        }

        public void Tick(float deltaTime)
        {
            if (!BattleAbilityManager.currentlyExecuting)
            {
                timer += deltaTime;
                OnAbilityTimerChanged?.Invoke(timer, GetCooldown(), timer / GetCooldown());
            }
        }

        public void ResetTimer()
        {
            timer = 0f;
            float currentCooldown = 0f;
            equipmentEvents?.OnGetStartingCooldowns?.Invoke(ref currentCooldown);
            timer = GetCooldown() * currentCooldown;
            OnAbilityTimerChanged?.Invoke(timer, GetCooldown(), timer / GetCooldown());
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
            if (CanBeUsed())
            {
                timer = 0f;
                executingAbility = true;
                allies[0].health.OnDeath += CancelExecution;
                yield return BattleAbilityManager.ExecuteAbility(this, allies, targets, level, animator, GetAnimationTrigger(), bloodRef);
                OnAbilityTimerChanged?.Invoke(0f, GetCooldown(), 0f);
                allies[0].health.OnDeath -= CancelExecution;
                executingAbility = false;
            }
        }

        public void Perform(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, ScriptableIntReference bloodRef = null)
        {
            foreach (var effect in effects)
            {
                effect.PerformEffect(allies, targets, level, equipmentEvents, bloodRef);
            }
        }

        public void CancelExecution(CharacterHealth sender)
        {
            if (executingAbility)
                BattleAbilityManager.CancelExecution(this);
        }

        public bool CanBeUsed()
        {
            return timer >= GetCooldown();
        }

        public float GetCooldown()
        {
            float temp = cooldown;
            equipmentEvents?.OnGetCooldown?.Invoke(ref temp);
            return temp;
        }

        private string GetAnimationTrigger()
        {
            return effects[0].GetAnimationTrigger();
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

        private void Stun(BattleStatusManager sender)
        {
            timer = 0f;
        }

        private void AdvanceCooldowns(BattleStatusManager sender)
        {
            timer = GetCooldown();
        }
    }
}