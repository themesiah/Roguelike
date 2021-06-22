using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Core;
using Laresistance.Data;

namespace Laresistance.Battle
{
    public class BattleEffectExchange : BattleEffect
    {
        public BattleEffectExchange(int power, EffectTargetType targetType, BattleStatusManager selfStatus, EffectData effectData) : base(power, targetType, selfStatus, effectData)
        {

        }

        public override EffectType EffectType => EffectType.Exchange;

        public override string GetAnimationTrigger()
        {
            return "Effect";
        }

        public override string GetEffectString(int level, EquipmentsContainer equipments)
        {
            return Texts.GetText("EFF_EXCHANGE_DESC");
        }

        public override string GetShortEffectString(int level, EquipmentsContainer equipments)
        {
            return "X";
        }

        protected override void PerformEffectOnTarget(BattleStatusManager target, int level, EquipmentsContainer equipments, ScriptableIntReference bloodRef)
        {
            foreach(var status in SelfStatus.GetAllStatus())
            {
                status.CopyTo(target.GetStatus(status.StatusType));
            }
            foreach (var status in target.GetAllStatus())
            {
                status.CopyTo(SelfStatus.GetStatus(status.StatusType));
            }
            SelfStatus.Cure();
            target.RemoveBuffs();
        }

        public override bool EffectCanBeUsedStunned()
        {
            return true;
        }
    }
}