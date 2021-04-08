using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectObtainEnergy : BattleEffect
    {
        public BattleEffectObtainEnergy(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.ObtainEnergy;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.EffectPower, power);
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        public float GetEnergyAmount(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments) / 100f;
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_OBTAIN_EN_DESC";
            return Texts.GetText(textId, new object[] { GetEnergyAmount(level, equipments), GetTargetString() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetEnergyAmount(level, equipments).ToString();
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.EffectBloodCost, bloodRef);
            target.AddEnergy(GetEnergyAmount(level, equipments));
        }
    }
}