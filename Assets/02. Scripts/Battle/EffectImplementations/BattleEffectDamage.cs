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

        protected virtual bool CanHealWithVampirism => true;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AttackPower, power);
            power = (int)(power * SelfStatus.GetValueModifier(StatusType.DamageModification));
            power = System.Math.Max(1, power);
            if (IsComboAbility())
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.ComboAbilityPower, power);
            }
            else
            {
                power = equipments.ModifyValue(Equipments.EquipmentSituation.NonComboAbilityPower, power);
            }
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            int damage = GetPower(level, equipments);
            PerformAttackEffect(target, damage, equipments, bloodRef);
        }

        protected int PerformAttackEffect(BattleStatusManager target, int damage, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.AttackBloodCost, bloodRef);
            int damageDone = 0;
            if (UnityEngine.Random.value <= SelfStatus.GetValueModifier(StatusType.Blind))
            {
                // Damage barrier
                damage -= (int)target.GetValueModifier(StatusType.Barrier);
                damage = System.Math.Max(damage, 1);
                // Deal damage (equipment shenanigans calculated in TakeDamage)
                damageDone = target.health.TakeDamage(damage, target.GetEquipmentsContainer(), equipments);
                // Check for equipment and buff retaliation and apply if necessary
                int retaliation = 0;
                retaliation = target.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.RetaliationDamage, retaliation);
                retaliation += (int)target.GetValueModifier(StatusType.Retaliation);
                if (retaliation > 0)
                {
                    SelfStatus.health.TakeDamage(retaliation, equipments, target.GetEquipmentsContainer());
                }
                if (CanHealWithVampirism && SelfStatus.GetStatus(StatusType.Vampirism).HaveBuff())
                {
                    float coeficient = SelfStatus.GetStatus(StatusType.Vampirism).GetValue();
                    int selfHeal = (int) (damageDone * coeficient);
                    SelfStatus.health.Heal(selfHeal);
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