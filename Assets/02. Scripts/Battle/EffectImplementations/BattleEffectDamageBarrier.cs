using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectDamageBarrier : BattleEffect
    {
        public BattleEffectDamageBarrier(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Barrier;

        protected override CalculatePower calculatePowerFunction => SelfStatus.battleStats.CalculateShield;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
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
            string textId = "EFF_BARRIER_DESC";
            int power = GetPower(level, equipments);
            return Texts.GetText(textId, new object[] { power, GameConstantsBehaviour.Instance.barrierBuffDuration.GetValue() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments).ToString();
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef)
        {
            target.ApplyStatusEffect(SelfStatus, StatusType.Barrier, GetPower(level, equipments));
        }
    }
}