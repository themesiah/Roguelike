using Laresistance.Behaviours;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class SpeedStatusEffect : StatusEffect
    {
        public class SpeedEffect
        {
            public float speedCoeficient;
            public float timer;
        }
        private List<SpeedEffect> speedEffectInstances;

        public SpeedStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {
            speedEffectInstances = new List<SpeedEffect>();
        }

        public override StatusType StatusType => StatusType.Speed;

        public override float GetValue()
        {
            float speedModifier = 1f;
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedEffectInstances[i];
                speedModifier *= se.speedCoeficient;
            }
            return speedModifier;
        }

        public override void AddValue(float value)
        {
            speedEffectInstances.Add(new SpeedEffect() { speedCoeficient = value, timer = 0f });
            if (value > 1f)
            {
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Speed, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue());
            } else
            {
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Slow, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue());
            }
        }

        public override void Tick(float delta)
        {
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                SpeedEffect se = speedEffectInstances[i];
                se.timer += delta;
                if (se.timer >= GameConstantsBehaviour.Instance.speedModifierDuration.GetValue())
                {
                    speedEffectInstances.Remove(se);
                }
            }
        }

        public override void Cure()
        {
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                if (speedEffectInstances[i].speedCoeficient < 1f)
                {
                    speedEffectInstances.RemoveAt(i);
                }
            }
        }

        public override void RemoveBuff()
        {
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                if (speedEffectInstances[i].speedCoeficient > 1f)
                {
                    speedEffectInstances.RemoveAt(i);
                }
            }
        }

        public override bool HaveBuff()
        {
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                if (speedEffectInstances[i].speedCoeficient > 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool HaveDebuff()
        {
            for (int i = speedEffectInstances.Count - 1; i >= 0; --i)
            {
                if (speedEffectInstances[i].speedCoeficient < 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var speedEffect in speedEffectInstances)
            {
                other.AddValue(speedEffect.speedCoeficient);
            }
        }
    }
}