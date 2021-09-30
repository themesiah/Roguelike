
namespace Laresistance.Battle
{
    public abstract class StatusEffect
    {
        protected BattleStatusManager statusManager;
        public StatusEffect(BattleStatusManager statusManager)
        {
            this.statusManager = statusManager;
        }
        public abstract void Tick(float delta);
        public abstract float GetValue();
        public abstract void AddValue(float value);
        public abstract StatusType StatusType { get; }
        public virtual void Cure() { }
        public virtual void RemoveBuff() { }
        public virtual bool HaveDebuff() { return false; }
        public virtual bool HaveBuff() { return false; }
        public virtual bool AppliesBuff() { return HaveBuff(); }
        public abstract void CopyTo(StatusEffect other);
    }
}