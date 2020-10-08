using System.Collections.Generic;
using System.Collections;
using Laresistance.Core;
using System.Text;
using GamedevsToolbox.ScriptableArchitecture.Localization;

namespace Laresistance.Battle
{
    public class BattleAbility
    {
        private static int MAX_EFFECTS = 2;
        private float cooldown;
        private List<BattleEffect> effects = default;
        private float timer = 0f;
        private EquipmentEvents equipmentEvents;

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
            var text = new ScriptableTextRef("ABILITY_COOLDOWN");
            builder.Append(text.GetText(GetCooldown()));
            return builder.ToString();
        }

        public void Tick(float deltaTime)
        {
            timer += deltaTime;
            OnAbilityTimerChanged?.Invoke(timer, cooldown, timer / cooldown);
        }

        public void ResetTimer()
        {
            timer = 0f;
        }

        public IEnumerator ExecuteAbility(BattleStatusManager[] allies, BattleStatusManager[] targets, int level)
        {
            if (CanBeUsed())
            {
                foreach (var effect in effects)
                {
                    effect.PerformEffect(allies, targets, level, equipmentEvents);
                }
                timer = 0f;
            }
            yield return null;
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
    }
}