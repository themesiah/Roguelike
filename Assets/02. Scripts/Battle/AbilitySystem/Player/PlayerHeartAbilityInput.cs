using Laresistance.Behaviours;
using Laresistance.Core;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public class PlayerHeartAbilityInput : PlayerAbilityInput
    {

        protected override float SpecificCardRenewCooldown()
        {
            return GameConstantsBehaviour.Instance.heartCardRenewCooldown.GetValue();
        }

        public PlayerHeartAbilityInput(Player player, BattleStatusManager battleStatus) : base(player, battleStatus)
        {
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
        }

        protected override void OnBattleStart()
        {
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        protected override bool CanExecuteAbilities()
        {
            return true;
        }
    }
}