using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectPercentDamage : BattleEffectDamage
    {
        public BattleEffectPercentDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.PercentDamage;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AttackPower, power);
            power = (int)(power * SelfStatus.GetValueModifier(StatusType.DamageModification));
            if (IsComboAbility())
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.ComboAbilityPower, power);
            }
            else
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.NonComboAbilityPower, power);
            }
            power = System.Math.Max(1, power);
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {

            int damagePercent = GetPower(level, equipments);
            int damage = (int)(target.health.GetCurrentHealth() * (float)damagePercent / 100f);
            // Deals half damage to boss types
            if (target.IsBossType)
            {
                damage = damage / 2;
            }
            PerformAttackEffect(target, damage, equipments, bloodRef);
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_PERCENT_DAMAGE_DESC";
            return Texts.GetText(textId, new object[] { GetPower(level, equipments), GetTargetString() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return string.Format("{0}%", GetPower(level, equipments).ToString());
        }

        public override string GetAnimationTrigger()
        {
            return "Attack";
        }
    }
}