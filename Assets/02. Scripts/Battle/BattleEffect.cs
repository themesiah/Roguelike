using System.Collections;

namespace Laresistance.Battle
{
    public abstract class BattleEffect
    {
        protected int Power;

        public BattleEffect(int power)
        {
            Power = power;
        }

        public virtual int GetPower(int level)
        {
            if (level <= 0)
                throw new System.Exception("Effect level should be greater than 0");
            return 0;
        }

        public abstract void PerformEffect(BattleStatusManager user, BattleStatusManager[] targets, int level);
    }
}