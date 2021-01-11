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
        public BattleEffectStun(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Stun;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.OnGetPower?.Invoke(ref power);
            equipmentEvents?.OnGetEffectPower?.Invoke(ref power);
            equipmentEvents?.OnGetEffectPowerFlat?.Invoke(ref power);
            power = (int)(power * SelfStatus.GetDamageModifier());
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_STUN_DESC";
            return Texts.GetText(textId, new object[] { GetSeconds(GetPower(level, equipmentEvents)), GetTargetString() });
        }

        public override string GetShortEffectString(int level, EquipmentEvents equipmentEvents)
        {
            return string.Format("{0}s", GetSeconds(GetPower(level, equipmentEvents)).ToString());
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            equipmentEvents?.OnGetAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetEffectAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAbilityBloodCostFlat?.Invoke(bloodRef);
            int power = GetPower(level, equipmentEvents);
            float seconds = GetSeconds(power);
            target.Stun(seconds);
        }

        private float GetSeconds(int power)
        {
            return (float)power / 100f;
        }
    }
}