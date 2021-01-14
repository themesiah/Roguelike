using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;

namespace Laresistance.Battle
{
    public class BattleEffectBlind : BattleEffect
    {
        public BattleEffectBlind(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.Blind;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int powerTemp = System.Math.Abs(100 - Power);
            int power = Mathf.CeilToInt(powerTemp * (1 + ((level - 1) * 0.05f)));
            equipmentEvents?.OnGetPower?.Invoke(ref power);
            equipmentEvents?.OnGetEffectPower?.Invoke(ref power);
            //equipmentEvents?.OnGetEffectPowerFlat?.Invoke(ref powerTemp);

            if (Power > 100)
            {
                power += 100;
            } else
            {
                power = 100 - power;
            }
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            equipmentEvents?.OnGetAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetEffectAbilityBloodCost?.Invoke(bloodRef);
            equipmentEvents?.OnGetAbilityBloodCostFlat?.Invoke(bloodRef);
            target.ApplyBlind(GetModifier(level, equipmentEvents));
        }

        private float GetModifier(int level, EquipmentEvents equipmentEvents)
        {
            return GetPower(level, equipmentEvents) / 100f;
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            float modifier = GetModifier(level, equipmentEvents);
            string textId = "EFF_BLIND_DESC";
            return Texts.GetText(textId, new object[] { GetTargetString(), modifier * 100f, GameConstantsBehaviour.Instance.speedModifierDuration.GetValue() });
        }

        public override string GetShortEffectString(int level, EquipmentEvents equipmentEvents)
        {
            float modifier = GetModifier(level, equipmentEvents);
            if (modifier > 1f)
            {
                modifier -= 1f;
            }
            else
            {
                modifier = 1f - modifier;
            }
            return modifier.ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }
    }
}