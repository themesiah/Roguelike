using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectSpeed : BattleEffect
    {
        public BattleEffectSpeed(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
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
            target.ApplySpeedModifier(GetModifier(level, equipmentEvents));
        }

        private float GetModifier(int level, EquipmentEvents equipmentEvents)
        {
            return GetPower(level, equipmentEvents) / 100f;
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            float modifier = GetModifier(level, equipmentEvents);
            string textId = "";
            if (modifier > 100f)
            {
                textId = "EFF_ACCEL_DESC";
                modifier -= 100f;
                
            } else
            {
                textId = "EFF_SLOW_DESC";
                modifier = 100f - modifier;
            }
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipmentEvents), BattleStatusManager.SPEED_MODIFIER_DURATION });
        }
    }
}