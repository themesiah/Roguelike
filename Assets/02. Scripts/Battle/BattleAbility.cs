using System.Collections.Generic;
using System.Collections;
using Laresistance.Core;
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;

namespace Laresistance.Battle
{
    public class BattleAbility
    {
        private static int MAX_EFFECTS = 2;
        private float cooldown;
        private List<BattleEffect> effects = default;
        private float timer = 0f;
        private EquipmentEvents equipmentEvents;
        private bool executingAbility = false;

        public delegate void OnAbilityTimerChangedHandler(float currentTimer, float cooldown, float percent);
        public event OnAbilityTimerChangedHandler OnAbilityTimerChanged;

        public BattleAbility(List<BattleEffect> effectsToSet, float cooldown, EquipmentEvents equipmentEvents = null)
        {
            if (effectsToSet == null || effectsToSet.Count == 0)
                throw new System.Exception("Abilities should have at least one effect");
            if (effectsToSet.Count > MAX_EFFECTS)
                throw new System.Exception("Abilities can only have up to " + MAX_EFFECTS + " effects");
            effects = effectsToSet;
            this.cooldown = cooldown;
            this.equipmentEvents = equipmentEvents;
        }

        public BattleAbility Copy()
        {
            BattleAbility ba = new BattleAbility(effects, GetCooldown(), equipmentEvents);
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
            builder.Append(Texts.GetText("ABILITY_COOLDOWN", GetCooldown()));
            return builder.ToString();
        }

        public void Tick(float deltaTime)
        {
            if (!BattleAbilityManager.currentlyExecuting)
            {
                timer += deltaTime;
                OnAbilityTimerChanged?.Invoke(timer, cooldown, timer / cooldown);
            }
        }

        public void ResetTimer()
        {
            timer = 0f;
        }

        public IEnumerator ExecuteAbility(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator)
        {
            // We need to actually start a new coroutine here because prioritary abilities can and should be processed BEFORE non prioritary abilities.
            CoroutineHelperBehaviour.GetInstance().StartCoroutine(ExecuteAbilityCoroutine(allies, targets, level, animator));
            yield return null;
        }

        public IEnumerator ExecuteAbilityCoroutine(BattleStatusManager[] allies, BattleStatusManager[] targets, int level, IBattleAnimator animator)
        {
            if (CanBeUsed())
            {
                timer = 0f;
                executingAbility = true;
                allies[0].health.OnDeath += CancelExecution;
                yield return BattleAbilityManager.ExecuteAbility(this, allies, targets, level, animator, GetAnimationTrigger());
                allies[0].health.OnDeath -= CancelExecution;
                executingAbility = false;
            }
        }

        public void Perform(BattleStatusManager[] allies, BattleStatusManager[] targets, int level)
        {
            foreach (var effect in effects)
            {
                effect.PerformEffect(allies, targets, level, equipmentEvents);
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
            equipmentEvents?.InvokeOnGetCooldown(ref temp);
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
    }
}