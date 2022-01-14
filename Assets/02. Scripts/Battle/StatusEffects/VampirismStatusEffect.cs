using Laresistance.Behaviours;
using UnityEngine;

namespace Laresistance.Battle
{
    public class VampirismStatusEffect : StatusEffect
    {
        private float vampirismTimer;
        private float coeficient;

        private float PreparationTime => GameConstantsBehaviour.Instance.vampirismDuration.GetValue();

        public VampirismStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            vampirismTimer = 0f;
        }

        public override StatusType StatusType => StatusType.Vampirism;

        public override bool IsGoodStatus(float value)
        {
            return true;
        }

        protected override void AddValue(float value)
        {
            vampirismTimer = PreparationTime;
            GetDuration(ref vampirismTimer, IsGoodStatus(value));
            coeficient = value;
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Vampirism, vampirismTimer);
        }

        public override float GetValue()
        {
            return coeficient;
        }

        public override void Tick(float delta)
        {
            if (vampirismTimer > 0f)
            {
                vampirismTimer -= delta;
                vampirismTimer = Mathf.Max(vampirismTimer, 0f);
            }
        }

        public override void RemoveBuff()
        {
            vampirismTimer = -1f;
        }

        public override bool HaveBuff()
        {
            return vampirismTimer > 0f;
        }

        public override bool AppliesBuff()
        {
            return vampirismTimer > 0f;
        }

        public override void CopyTo(StatusEffect other)
        {
        }

        public override void RemoveStatus()
        {
            RemoveBuff();
        }
    }
}