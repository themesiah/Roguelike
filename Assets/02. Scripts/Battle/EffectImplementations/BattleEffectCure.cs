using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectCure : BattleEffect
    {
        public BattleEffectCure(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Cure;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            return 1;
        }

        public override bool EffectCanBeUsedStunned()
        {
            return true;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            equipments.ModifyValue(Equipments.EquipmentSituation.AbilityBloodCost, bloodRef);
            equipments.ModifyValue(Equipments.EquipmentSituation.EffectBloodCost, bloodRef);
            target.Cure();
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            string textId = "EFF_CURE_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipments)});
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            //return GetPower(level, equipments).ToString();
            return "X";
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override bool IsPrioritary => true;
    }
}