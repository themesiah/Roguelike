using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Behaviours;
using Laresistance.Core;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Laresistance.Battle
{
    public class BattleEffectSiphon : BattleEffectDamage
    {
        public BattleEffectSiphon(int power, EffectTargetType targetType, BattleStatusManager selfStatus) : base(power, targetType, selfStatus)
        {

        }

        public override EffectType EffectType => EffectType.LifeSiphon;

        public override int GetPower(int level, EquipmentEvents equipmentEvents)
        {
            base.GetPower(level, equipmentEvents);
            int power = Mathf.CeilToInt(Power * (1 + ((level - 1) * 0.1f)));
            equipmentEvents?.OnGetPower?.Invoke(ref power);
            equipmentEvents?.OnGetAttackPower?.Invoke(ref power);
            equipmentEvents?.OnGetAttackPowerFlat?.Invoke(ref power);
            power = (int)(power * SelfStatus.GetDamageModifier());
            power = System.Math.Max(1, power);
            Assert.IsTrue(power >= 0, "Power should not be negative.");
            return power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentEvents equipmentEvents, ScriptableIntReference bloodRef = null)
        {
            int damageDone = PerformAttackEffect(target, level, equipmentEvents, bloodRef);
            if (damageDone > 0)
            {
                int healAmount = Mathf.FloorToInt(damageDone * GameConstantsBehaviour.Instance.siphonPercent.GetValue());
                if (healAmount > 0)
                {
                    SelfStatus.health.Heal(healAmount);
                }
            }
        }

        public override string GetEffectString(int level, EquipmentEvents equipmentEvents)
        {
            string textId = "EFF_SIPHON_DESC";
            return Texts.GetText(textId, new object[] { GetPower(level, equipmentEvents), GetTargetString(), GameConstantsBehaviour.Instance.siphonPercent.GetValue()*100f });
        }

        public override string GetShortEffectString(int level, EquipmentEvents equipmentEvents)
        {
            return GetPower(level, equipmentEvents).ToString();
        }

        public override string GetAnimationTrigger()
        {
            return "Attack";
        }
    }
}