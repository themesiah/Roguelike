using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectDamageModification : BattleEffect
    {
        public BattleEffectDamageModification(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.DamageModification;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = 0;
            if (Power > 100)
            {
                power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            } else
            {
                int reversePower = 100 - Power;
                reversePower = Mathf.CeilToInt(reversePower * (1 + ((level - 1) * 0.1f)));
                power = 100 - reversePower;
            }
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.EffectPower, power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.EffectBloodCost, bloodRef);
            target.ApplyStatusEffect(StatusType.DamageModification, (float)GetPower(level, equipments) / 100f);
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_DAMAGE_IMPRV_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipments) });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            int power = GetPower(level, equipments);
            if (power > 100)
            {
                return string.Format("{0}%", power);
            } else
            {
                return string.Format("-{0}%", 100 - power);
            }
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}