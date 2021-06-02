using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectStun : BattleEffect
    {
        public BattleEffectStun(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Stun;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            power = equipments.ModifyValue(Equipments.EquipmentSituation.EffectPower, power);
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_STUN_DESC";
            return Texts.GetText(textId, new object[] { GetSeconds(GetPower(level, equipments)), GetTargetString() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return string.Format("{0}s", GetSeconds(GetPower(level, equipments)).ToString("0.0"));
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.EffectBloodCost, bloodRef);
            int power = GetPower(level, equipments);
            float seconds = GetSeconds(power);
            target.Stun(seconds);
        }

        private float GetSeconds(int power)
        {
            return (float)power / 100f;
        }
    }
}