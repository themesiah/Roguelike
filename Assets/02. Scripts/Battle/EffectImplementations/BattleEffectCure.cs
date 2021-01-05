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
        public BattleEffectCure(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Cure;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            return 0;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            equipmentEvents?.OnGetAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetEffectAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAbilityBloodCostFlat?.Invoke(bloodRef);
            target.Cure();
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_CURE_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), GetPower(level, equipmentEvents)});
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