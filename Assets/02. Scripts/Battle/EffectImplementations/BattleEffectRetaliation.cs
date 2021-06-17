using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectRetaliation : BattleEffect
    {
        public BattleEffectRetaliation(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Retaliation;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            power = equipments.ModifyValue(Equipments.EquipmentSituation.AbilityPower, power);
            return power;
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_RETALIATION_DESC";
            int power = GetPower(level, equipments);
            return Texts.GetText(textId, new object[] { power, GameConstantsBehaviour.Instance.retaliationBuffDuration.GetValue() });
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return GetPower(level, equipments).ToString();
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef)
        {
            target.ApplyStatusEffect(StatusType.Retaliation, GetPower(level, equipments));
        }
    }
}