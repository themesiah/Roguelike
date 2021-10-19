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

        public override void AddValue(float value)
        {
            active = true;
            timer = 0f;
            statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Rush, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue());
        }

        public override void Tick(float delta)
        {
            if (active)
            {
                timer += delta;
                if (timer >= GameConstantsBehaviour.Instance.speedModifierDuration.GetValue())
                {
                    active = false;
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
                other.AddValue(0f);
        }
    }
}