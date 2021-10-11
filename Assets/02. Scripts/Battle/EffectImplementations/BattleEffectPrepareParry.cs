using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class BattleEffectPrepareParry : BattleEffect
    {
        public BattleEffectPrepareParry(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.PrepareParry;

        public override int GetPower(int level, EquipmentsContainer equipments)
        {
            return 1;
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef = null)
        {
            target.PrepareParry();
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
            return "Parry";
        }
    }
}