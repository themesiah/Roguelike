using Laresistance.Behaviours;

namespace Laresistance.Battle
{
    public class RushStatusEffect : StatusEffect
    {
        private bool active = false;
        private float timer = 0f;

        public RushStatusEffect(BattleStatusManager statusManager) : base(statusManager)
        {
        }

        public override StatusType StatusType => StatusType.Rush;

        public override float GetValue()
        {
            return active ? 1f : 0f;
        }

        protected override void AddValue(float value)
        {
            active = true;
            timer = GameConstantsBehaviour.Instance.bodyRushBuffDuration.GetValue();
            GetDuration(ref timer);
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Rush, timer);
        }

        public override void RealtimeTick(float delta)
        {
            if (active)
            {
                timer -= delta;
                if (timer <= 0f)
                {
                    active = false;
                    statusManager.OnSingletonStatusRemoved?.Invoke(statusManager, StatusIconType.Rush);
                }
            }
        }

        public bool HaveRush()
        {
            return active;
        }

        public void RemoveRush()
        {
            active = false;
            statusManager.OnSingletonStatusRemoved?.Invoke(statusManager, StatusIconType.Rush);
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            if (active)
                other.AddValue(sourceStatusManager, 0f);
        }

        public override void RemoveStatus()
        {
            RemoveRush();
        }
    }
}