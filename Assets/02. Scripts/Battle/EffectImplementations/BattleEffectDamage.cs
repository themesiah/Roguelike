using GamedevsToolbox.ScriptableArchitecture.Localization;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectDamage : BattleEffect
    {
        public BattleEffectDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Damage;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            power = (int)(power * SelfStatus.GetDamageModifier());
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents)
        {
            target.health.TakeDamage(GetPower(level, equipmentEvents));
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_DAMAGE_DESC";
            var text = new ScriptableTextRef(textId);
            return text.GetText(GetPower(level, equipmentEvents), GetTargetString());
        }
    }
}