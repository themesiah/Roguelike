using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public abstract class BattleEffect
    {
        protected int Power;

        public BattleEffect(int power)
        {
            SetPower(power);
        }

        public virtual int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            if (level <= 0)
                throw new System.Exception("Effect level should be greater than 0");
            return 0;
        }

        public void SetPower(int power)
        {
            Power = power;
        }

        public abstract void PerformEffect(BattleStatusManager user, BattleStatusManager[] targets, int level, EquipmentEvents equipmentEvents);
        public abstract EffectType EffectType { get; }
    }
}