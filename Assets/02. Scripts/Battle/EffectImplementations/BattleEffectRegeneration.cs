using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectRegeneration : BattleEffect
    {
        public BattleEffectRegeneration(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Regeneration;

        protected override CalculatePower calculatePowerFunction => SelfStatus.battleStats.CalculateHeal;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.HealPower, power);
            if (IsComboAbility())
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.ComboAbilityPower, power);
            }
            else
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.NonComboAbilityPower, power);
            }
            return power;
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_REGEN_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipments), GameConstantsBehaviour.Instance.healOverTimeTickDelay.GetValue(), GameConstantsBehaviour.Instance.healOverTimeDuration.GetValue() });

        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments).ToString();
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef)
        {
            target.ApplyStatusEffect(SelfStatus, StatusType.Regeneration, GetPower(level, equipments));
        }
    }
}