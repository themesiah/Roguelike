using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectSiphon : BattleEffectDamage
    {
        public BattleEffectSiphon(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.LifeSiphon;

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
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.AttackBloodCost, bloodRef);
            int damage = GetPower(level, equipments);
            int damageDone = PerformAttackEffect(target, damage, equipments, bloodRef);
            if (damageDone > 0)
            {
                int healAmount = Mathf.FloorToInt(damageDone * GameConstantsBehaviour.Instance.siphonPercent.GetValue());
                if (healAmount > 0)
                {
                    healAmount = equipments.ModifyValue(Equipments.EquipmentSituation.HealPower, healAmount);
                    SelfStatus.health.Heal(healAmount);
                }
            }
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_SIPHON_DESC";
            return Texts.GetText(textId, new object[] { GetPower(level, equipments), GetTargetString(), GameConstantsBehaviour.Instance.siphonPercent.GetValue()*100f });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments).ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Attack";
        }
    }
}