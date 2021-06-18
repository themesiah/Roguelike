using Laresistance.Behaviours;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class DamageModificationStatusEffect : StatusEffect
    {
        public class TempDamageChange
        {
            public float modifier;
            public float timer;
        }

        private List<TempDamageChange> damageModifications;

        public DamageModificationStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            damageModifications = new List<TempDamageChange>();
        }

        public override StatusType StatusType => StatusType.DamageModification;

        public override void AddValue(float value)
        {
            damageModifications.Add(new TempDamageChange() { modifier = value, timer = 0f });
            if (value > 1f) {
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.DamageImprovement, -1f);
            } else {
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Debuff, GameConstantsBehaviour.Instance.damageModifierDuration.GetValue());
            }
        }

        public override float GetValue()
        {
            float damageModifier = 1f;

            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                var modifier = damageModifications[i];
                damageModifier *= modifier.modifier;
            }

            return damageModifier;
        }

        public override void Tick(float delta)
        {
            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                if (damageModifications[i].modifier < 1f)
                {
                    TempDamageChange tdm = damageModifications[i];
                    tdm.timer += delta;
                    if (tdm.timer >= GameConstantsBehaviour.Instance.damageModifierDuration.GetValue())
                    {
                        damageModifications.Remove(tdm);
                    }
                }
            }
        }

        public override void Cure()
        {
            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                if (damageModifications[i].modifier < 1f)
                {
                    damageModifications.RemoveAt(i);
                }
            }
        }

        public override void RemoveBuff()
        {
            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                if (damageModifications[i].modifier > 1f)
                {
                    damageModifications.RemoveAt(i);
                }
            }
        }

        public override bool HaveDebuff()
        {
            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                if (damageModifications[i].modifier < 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool HaveBuff()
        {
            for (int i = damageModifications.Count - 1; i >= 0; --i)
            {
                if (damageModifications[i].modifier > 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var damageModification in damageModifications)
            {
                other.AddValue(damageModification.modifier);
            }
        }
    }
}