using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectImproveDamage : BattleEffect
    {
        public BattleEffectImproveDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.ImproveDamage;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.InvokeOnGetPower(ref power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents)
        {
            target.ApplyDamageImprovement(((float)GetPower(level, equipmentEvents) + 100f)/100f);
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_DAMAGE_IMPRV_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipmentEvents) });
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}