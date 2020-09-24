using System.Collections;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectShield : BattleEffect
    {
        public BattleEffectShield(int power) : base(power)
        {

        }

        public override int GetPower(int level)
        {
            base.GetPower(level);
            return Mathf.CeilToInt(Power * (1+((level - 1) * 0.1f)));
        }

        public override void PerformEffect(BattleStatusManager user, BattleStatusManager[] targets, int level)
        {
            targets[0].health.AddShield(GetPower(level));
        }
    }
}