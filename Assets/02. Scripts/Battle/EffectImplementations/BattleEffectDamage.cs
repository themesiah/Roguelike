using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectDamage : BattleEffect
    {
        public BattleEffectDamage(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Damage;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.OnGetPower?.Invoke(ref power);
            equipmentEvents?.OnGetAttackPower?.Invoke(ref power);
            equipmentEvents?.OnGetAttackPowerFlat?.Invoke(ref power);
            power = (int)(power * SelfStatus.GetDamageModifier());
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            equipmentEvents?.OnGetAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAttackAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAbilityBloodCostFlat?.Invoke(bloodRef);
            equipmentEvents?.OnGetAttackAbilityBloodCostFlat?.Invoke(bloodRef);
            int damage = GetPower(level, equipmentEvents);
            target.health.TakeDamage(damage, equipmentEvents);
            int retaliation = 0;
            equipmentEvents?.OnRetaliationDamage?.Invoke(ref damage, ref retaliation);
            equipmentEvents?.OnRetaliationDamageFlat?.Invoke(ref damage, ref retaliation);
            if (retaliation > 0)
            {
                SelfStatus.health.TakeDamage(damage, null);
            }
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_DAMAGE_DESC";
            return Texts.GetText(textId, new object[] { GetPower(level, equipmentEvents), GetTargetString() });
        }

        public override string GetAnimationTrigger()
        {
            return "Attack";
        }
    }
}