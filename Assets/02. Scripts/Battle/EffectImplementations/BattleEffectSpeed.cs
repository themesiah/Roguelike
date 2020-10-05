using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectSpeed : BattleEffect
    {
        public BattleEffectSpeed(int power, EffectTargetType targetType) : base(power, targetType)
        {

        }

        public override EffectType EffectType => EffectType.Speed;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.05f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents)
        {
            target.ApplySpeedModifier(GetPower(level, equipmentEvents) / 100f);
        }
    }
}