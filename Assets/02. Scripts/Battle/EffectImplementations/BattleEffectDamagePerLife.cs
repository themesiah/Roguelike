using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectDamagePerLife : BattleEffectDamage
    {
        public BattleEffectDamagePerLife(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.DamagePerLife;

        protected override CalculatePower calculatePowerFunction => SelfStatus.battleStats.CalculateDamage;

        protected override bool CanHealWithVampirism => false;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AttackPower, power);
            power = (int)(power * SelfStatus.GetValueModifier(StatusType.DamageModification));
            power = System.Math.Max(1, power);
            int totalPower = (int)((float)power * SelfStatus.health.GetPercentHealth());
            Assert.IsTrue(totalPower >= 0, "Power should not be negative.");
            return totalPower;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            int damage = GetPower(level, equipments);
            PerformAttackEffect(target, damage, equipments, bloodRef);
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            return "";
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return "";
        }

        public override string GetAnimationTrigger()
        {
            return "Attack";
        }
    }
}