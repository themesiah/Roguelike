namespace Laresistance.Battle
{
    public class StunStatusEffect : StatusEffect
    {
        public StunStatusEffect(BattleStatusManager statusManager) : base(statusManager) {
            stunTimer = 0f;
        }
        public override StatusType StatusType => StatusType.Stun;

        private float stunTimer;

        public override void AddValue(float value)
        {
            if (stunTimer < value)
            {
                stunTimer = value;
                statusManager.OnStatusApplied?.Invoke(statusManager, StatusIconType.Stun, value);
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
    }
}