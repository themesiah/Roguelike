﻿using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectBlind : BattleEffect
    {
        public BattleEffectBlind(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Blind;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int powerTemp = System.Math.Abs(100 - Power);
            int power = Mathf.CeilToInt(powerTemp * (1 + ((level - 1) * 0.05f)));

            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.EffectPower, power);

            if (Power > 100)
            {
                power += 100;
            } else
            {
                power = 100 - power;
            }
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.EffectBloodCost, bloodRef);
            target.ApplyBlind(GetModifier(level, equipments));
        }

        private float GetModifier(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments) / 100f;
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            float modifier = GetModifier(level, equipments);
            string textId = "EFF_BLIND_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), modifier * 100f, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            float modifier = GetModifier(level, equipments);
            if (modifier > 1f)
            {
                modifier -= 1f;
            }
            else
            {
                modifier = 1f - modifier;
            }
            return modifier.ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}