using Laresistance.Behaviours;
using System.Collections.Generic;

namespace Laresistance.Battle
{
    public class RetaliationStatusEffect : StatusEffect
    {
        public class RetaliationEffect
        {
            public int retaliationDamage;
            public float timer;
        }
        private List<RetaliationEffect> retaliationEffectInstances;

        public RetaliationStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {
            retaliationEffectInstances = new List<RetaliationEffect>();
        }

        public override StatusType StatusType => StatusType.Retaliation;

        protected override void AddValue(float value)
        {
            float duration = GameConstantsBehaviour.Instance.retaliationBuffDuration.GetValue();
            GetDuration(ref duration);
            retaliationEffectInstances.Add(new RetaliationEffect() { retaliationDamage = (int)value, timer = duration });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Retaliation, duration);
        }

        public override float GetValue()
        {
            int totalDamage = 0;
            foreach(var retaliationEffect in retaliationEffectInstances)
            {
                totalDamage += retaliationEffect.retaliationDamage;
            }
            return totalDamage;
        }

        public override void Tick(float delta)
        {
            for (int i = retaliationEffectInstances.Count - 1; i >= 0; --i)
            {
                RetaliationEffect se = retaliationEffectInstances[i];
                se.timer -= delta;
                if (se.timer <= 0f)
                {
                    retaliationEffectInstances.Remove(se);
                }
            }
        }

        public override bool HaveBuff()
        {
            return retaliationEffectInstances.Count > 0;
        }

        public override void RemoveBuff()
        {
            retaliationEffectInstances.Clear();
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            foreach (var retaliationEffect in retaliationEffectInstances)
            {
                other.AddValue(sourceStatusManager, retaliationEffect.retaliationDamage);
            }
        }

        public override void RemoveStatus()
        {
            RemoveBuff();
        }
    }
}