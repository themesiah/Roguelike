using Laresistance.Core;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public class PlayerBodyAbilityInput : PlayerAbilityInput
    {
        private bool delayedBlockStop = false;
        public PlayerBodyAbilityInput(Player player, BattleStatusManager battleStatus) : base(player, battleStatus)
        {
            battleStatus.OnStatusApplied += OnStatusApplied;
            battleStatus.health.OnAttackReceived += AttackReceived;
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryToExecuteAbility(5);
            }
            if (context.canceled)
            {
                if (((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock())
                {
                    ((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).CancelBlock();
                } else
                {
                    delayedBlockStop = true;
                }
                
            }
        }

        protected override void OnBattleStart()
        {
            ExecuteOnNextSupportAbilityProgress(1f);
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        private void OnStatusApplied(BattleStatusManager sender, StatusIconType statusType, float duration)
        {
            if (statusType == StatusIconType.Stun)
            {
                if (((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock())
                {
                    ((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).CancelBlock();
                }
            }
        }

        protected override bool CanExecuteAbilities()
        {
            return !(((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock());
        }

        protected override void ModifyInputDelta(ref float delta)
        {
            if (((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock())
            {
                delta = 0f;
            }
        }

        private void AttackReceived(CharacterHealth sender)
        {
            battleStatus.AddEnergy(10f);
        }

        protected override void OnAbilitiesUpdate(float delta)
        {
            if (((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock() && delayedBlockStop)
            {
                ((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).CancelBlock();
                delayedBlockStop = false;
            }
        }

        protected override void OnAbilityExecutedExtra(BattleAbility ability, int slot)
        {
            if ((((RushStatusEffect)battleStatus.GetStatus(StatusType.Rush)).HaveRush()))
            {
                renewTimer = 0f;
            }
        }
    }
}