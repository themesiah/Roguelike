using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class BattleEffectPrepareBlock : BattleEffect
    {
        public BattleEffectPrepareBlock(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.PrepareBlock;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            return 1;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            target.PrepareShield();
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