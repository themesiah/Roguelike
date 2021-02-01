using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectShield : BattleEffect
    {
        public BattleEffectShield(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Shield;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.ShieldPower, power);
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.ShieldBloodCost, bloodRef);
            target.health.AddShield(GetPower(level, equipments));
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_SHIELD_DESC";
            return Texts.GetText(textId, GetPower(level, equipments));
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments).ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Shield";
        }

        public override bool IsPrioritary => true;
    }
}