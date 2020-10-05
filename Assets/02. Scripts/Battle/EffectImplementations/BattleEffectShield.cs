using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectShield : BattleEffect
    {
        public BattleEffectShield(int power, EffectTargetType targetType) : base(power, targetType)
        {

        }

        public override EffectType EffectType => EffectType.Shield;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents)
        {
            target.health.AddShield(GetPower(level, equipmentEvents));
        }
    }
}