using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectDamage : BattleEffect
    {
        public BattleEffectDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Damage;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AttackPower, power);
            power = (int)(power * SelfStatus.GetDamageModifier());
            power = System.Math.Max(1, power);
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            PerformAttackEffect(target, level, equipments, bloodRef);
        }

        protected int PerformAttackEffect(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.AttackBloodCost, bloodRef);
            int damageDone = 0;
            if (UnityEngine.Random.value <= SelfStatus.GetHitChance())
            {
                int damage = GetPower(level, equipments);
                damageDone = target.health.TakeDamage(damage, target.GetEquipmentsContainer(), equipments);
                int retaliation = 0;
                retaliation = target.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.RetaliationDamage, retaliation);
                if (retaliation > 0)
                {
                    SelfStatus.health.TakeDamage(retaliation, equipments, target.GetEquipmentsContainer());
                }
            } else
            {
                target.health.MissAttack();
            }
            return damageDone;
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_DAMAGE_DESC";
            return Texts.GetText(textId, new object[] { GetPower(level, equipments), GetTargetString() });
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