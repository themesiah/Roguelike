
namespace Laresistance.Battle
{
    public abstract class StatusEffect
    {
        protected BattleStatusManager statusManager;
        protected BattleStatusManager sourceStatusManager;

        public StatusEffect(BattleStatusManager statusManager)
        {
            this.statusManager = statusManager;
        }
        public virtual void Tick(float delta) { }
        public virtual void RealtimeTick(float delta) { }
        public abstract float GetValue();
        public void AddValue(BattleStatusManager sourceStatusManager, float value)
        {
            this.sourceStatusManager = sourceStatusManager;
            AddValue(value);
        }

        protected abstract void AddValue(float value);

        public abstract StatusType StatusType { get; }
        public virtual void Cure() { }
        public virtual void RemoveBuff() { }
        public virtual bool HaveDebuff() { return false; }
        public virtual bool HaveBuff() { return false; }
        public virtual bool AppliesBuff() { return HaveBuff(); }
        public abstract void RemoveStatus();
        public abstract void CopyTo(StatusEffect other);

        public abstract bool IsGoodStatus(float value);

        protected virtual void GetDuration(ref float duration, bool goodStatus)
        {
            duration = sourceStatusManager.battleStats.CalculateStatusTime(duration);
            duration = sourceStatusManager.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.StatusDuration, duration);
            if (goodStatus)
            {
                // Nothing for now
            } else
            {
                duration = statusManager.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.SelfNegativeStatusDuration, duration);
            }
        }
    }
}