using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class BattleEffectBodyBlock : BattleEffect
    {
        public BattleEffectBodyBlock(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.BodyBlock;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            base.GetPower(level, equipments);
            return Power;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            float percentPower = (float)GetPower(level, equipments) / 100f;
            target.ApplyStatusEffect(SelfStatus, StatusType.BodyBlockStatus, percentPower);
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            return "";
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return "";
        }

        public override string GetAnimationTrigger()
        {
            return "Block";
        }
    }
}