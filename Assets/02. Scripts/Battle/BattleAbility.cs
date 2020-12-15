using System.Collections.Generic;
using System.Collections;
using Laresistance.Core;
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using GamedevsToolbox.ScriptableArchitecture.Values;
using System.Diagnostics;
using UnityEngine;
using Laresistance.Data;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif

namespace Laresistance.Battle
{
    public class BattleAbility
    {
        private static int MAX_EFFECTS = 2;
        private int energyCost;
        private List<BattleEffect> effects = default;
        private EquipmentEvents equipmentEvents;
        private BattleStatusManager statusManager;
        private bool executingAbility = false;
        private float cooldownTimer = 0f;
        private AbilityData data;
        public int Weight { get; private set; }
        public float Cooldown { get; private set; }
        public bool IsShieldAbility { get; private set; }
        public bool IsOffensiveAbility { get; private set; }
        public bool IsBasicSkill { get; private set; }
        public Sprite AbilityIcon { get; private set; }

        public BattleAbility(List<BattleEffect> effectsToSet, int energyCost, int weight, float cooldown, BattleStatusManager statusManager, EquipmentEvents equipmentEvents = null, Sprite icon = null, AbilityData data = null)
        {
            if (effectsToSet == null || effectsToSet.Count == 0)
                throw new System.Exception("Abilities should have at least one effect");
            if (effectsToSet.Count > MAX_EFFECTS)
                throw new System.Exception("Abilities can only have up to " + MAX_EFFECTS + " effects");
            effects = effectsToSet;
            this.equipmentEvents = equipmentEvents;
            this.statusManager = statusManager;
            this.energyCost = energyCost;
            this.Weight = weight;
            this.Cooldown = cooldown;
            this.AbilityIcon = icon;
            this.data = data;
        }

        public BattleAbility Copy()
        {
            var ability = new BattleAbility(effects, energyCost, Weight, Cooldown, statusManager, equipmentEvents);
            if (IsShieldAbility)
            {
                ability.SetShieldAbility();
            }
            if (IsOffensiveAbility)
            {
                ability.SetOffensiveAbility();
            }
            if (IsBasicSkill)
            {
                ability.SetBasicSkill();
            }

            return ability;
        }

        public void SetShieldAbility()
        {
            IsShieldAbility = true;
        }

        public void SetOffensiveAbility()
        {
            IsOffensiveAbility = true;
        }

        public void SetBasicSkill()
        {
            IsBasicSkill = true;
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
        }

        public BattleStatusManager GetStatusManager()
        {
            return this.statusManager;
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
            if (GetCost() > 0)
                builder.Append(Texts.GetText("ABILITY_COOLDOWN", GetCost()));
            return builder.ToString();
        }

        public string GetShortAbilityText()
        {
            return Texts.GetText(data.ShortDesc);
        }

        public void Tick(float deltaTime)
        {
            if (!BattleAbilityManager.Executing && cooldownTimer > 0f)
            {
                cooldownTimer -= deltaTime * statusManager.GetSpeedModifier();
            }
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
                executingAbility = true;
                allies[0].health.OnDeath += CancelExecution;
                statusManager.ConsumeEnergy(energyCost);
                SetCooldownAsUsed();
                yield return BattleAbilityManager.ExecuteAbility(this.Copy(), allies, targets, level, animator, GetAnimationTrigger(), bloodRef);
                allies[0].health.OnDeath -= CancelExecution;
                executingAbility = false;
            }
        }

        public void Perform(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator, ScriptableIntReference bloodRef = null)
        {
            foreach (var effect in effects)
            {
                effect.PerformEffect(allies, targets, level, equipmentEvents, animator, bloodRef);
            }
        }

        public void SetCooldownAsUsed()
        {
            cooldownTimer = Cooldown;
        }

        public void CancelExecution(CharacterHealth sender)
        {
            if (executingAbility)
                BattleAbilityManager.CancelExecution(this);
        }

        public bool CanBeUsed()
        {
            if (IsBasicSkill)
            {
                return statusManager.CanExecute(energyCost) && !BattleAbilityManager.Executing;
            }
            else
            {
                return statusManager.CanExecute(energyCost) && cooldownTimer <= 0f;
            }
        }

        public int GetCost()
        {
            return energyCost;
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
    }
}