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
        public BattleEffectDamageModification(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.DamageModification;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.OnGetPower?.Invoke(ref power);
            equipmentEvents?.OnGetEffectPower?.Invoke(ref power);
            equipmentEvents?.OnGetEffectPowerFlat?.Invoke(ref power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            equipmentEvents?.OnGetAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetEffectAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAbilityBloodCostFlat?.Invoke(bloodRef);
            target.ApplyTempDamageModification((float)GetPower(level, equipmentEvents) / 100f);
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_DAMAGE_IMPRV_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipmentEvents) });
        }

        public override string GetShortEffectString(int level, EquipmentEvents equipmentEvents)
        {
            return GetPower(level, equipmentEvents).ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}