using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Laresistance.Battle
{
    public class BattleAbility
    {
        private static int MAX_EFFECTS = 2;
        public float Cooldown { get; private set; }
        private List<BattleEffect> effects = default;
        private float lastExecution = 0f;

        public BattleAbility(List<BattleEffect> effectsToSet, float cooldown)
        {
            if (effectsToSet.Count > MAX_EFFECTS)
                throw new System.Exception("Abilities can only have up to " + MAX_EFFECTS + " effects");
            effects = effectsToSet;
            Cooldown = cooldown;
        }

        public int GetEffectPower(int index, int level)
        {
            if (index > effects.Count - 1 || index < 0)
                throw new System.Exception("Invalid index for effect power");
            return effects[index].GetPower(level);
        }

        public IEnumerator ExecuteAbility(BattleStatusManager user, BattleStatusManager[] targets, int level)
        {
            if (CanBeUsed())
            {
                foreach (var effect in effects)
                {
                    effect.PerformEffect(user, targets, level);
                }
                lastExecution = Time.time;
            }
            yield return null;
        }

        public bool CanBeUsed()
        {
            return Time.time > lastExecution + Cooldown;
        }
    }
}