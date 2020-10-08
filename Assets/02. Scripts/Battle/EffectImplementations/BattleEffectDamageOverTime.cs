using GamedevsToolbox.ScriptableArchitecture.Localization;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectDamageOverTime : BattleEffect
    {
        public BattleEffectDamageOverTime(int power, EffectTargetType targetType) : base(power, targetType)
        {

        }

        public override EffectType EffectType => EffectType.DamageOverTime;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents)
        {
            target.ApplyDamageOverTime(GetPower(level, equipmentEvents));
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_DOT_DESC";
            var text = new ScriptableTextRef(textId);
            return text.GetText(new object[] { GetTargetString(), GetPower(level, equipmentEvents), BattleStatusManager.DAMAGE_OVER_TIME_TICK_DELAY, BattleStatusManager.DAMAGE_OVER_TIME_DURATION });
        }
    }
}