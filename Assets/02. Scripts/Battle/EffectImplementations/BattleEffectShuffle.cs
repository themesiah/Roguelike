using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class BattleEffectShuffle : BattleEffect
    {
        public BattleEffectShuffle(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Shuffle;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            return 0;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            target.ExecuteSupportAbility();
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
            return "Effect";
        }
    }
}