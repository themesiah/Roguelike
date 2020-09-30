using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectHeal : BattleEffect
    {
        public BattleEffectHeal(int power) : base(power)
        {

        }

        public override EffectType EffectType => EffectType.Heal;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            return power;
        }

        public override void PerformEffect(BattleStatusManager user, BattleStatusManager[] targets, int level, EquipmentEvents equipmentEvents)
        {
            targets[0].health.Heal(GetPower(level, equipmentEvents));
        }
    }
}