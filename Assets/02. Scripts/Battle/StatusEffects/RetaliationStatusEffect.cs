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

        public override void AddValue(float value)
        {
            retaliationEffectInstances.Add(new RetaliationEffect() { retaliationDamage = (int)value, timer = 0f });
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Retaliation, GameConstantsBehaviour.Instance.retaliationBuffDuration.GetValue());
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
                se.timer += delta;
                if (se.timer >= GameConstantsBehaviour.Instance.retaliationBuffDuration.GetValue())
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
    }
}