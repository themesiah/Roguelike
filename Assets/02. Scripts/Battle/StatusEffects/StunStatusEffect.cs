namespace Laresistance.Battle
{
    public class StunStatusEffect : StatusEffect
    {
        public StunStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            stunTimer = 0f;
        }
        public override StatusType StatusType => StatusType.Stun;

        public override bool IsGoodStatus(float value)
        {
            return false;
        }

        private float stunTimer;

        protected override void AddValue(float value)
        {
            if (value > 0f && stunTimer < value)
            {
                stunTimer = value;
                GetDuration(ref stunTimer, IsGoodStatus(value));
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Stun, stunTimer);
            }
        }

        public override float GetValue()
        {
            return stunTimer;
        }

        public override void Tick(float delta)
        {
            if (stunTimer > 0f)
            {
                stunTimer -= delta;
            }
        }

        public override void Cure()
        {
            stunTimer = 0f;
        }

        public override bool HaveDebuff()
        {
            return stunTimer > 0f;
        }

        public override void CopyTo(StatusEffect other)
        {
            UnityEngine.Assertions.Assert.AreEqual(StatusType, other.StatusType);
            other.AddValue(sourceStatusManager ,stunTimer);
        }

        public override void RemoveStatus()
        {
            Cure();
        }
    }
}