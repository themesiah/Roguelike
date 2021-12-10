﻿using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectImproveDamage : BattleEffect
    {
        public BattleEffectImproveDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.ImproveDamage;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.EffectPower, power);
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

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.AttackBloodCost, bloodRef);
            target.ApplyStatusEffect(SelfStatus, StatusType.DamageModification, ((float)GetPower(level, equipments) + 100f) / 100f);
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_DAMAGE_IMPRV_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipments) });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return string.Format("{0}%", GetPower(level, equipments));
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}