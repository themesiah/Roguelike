using Laresistance.Behaviours;
using Laresistance.Core;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public class PlayerBodyAbilityInput : PlayerAbilityInput
    {
        private static float SUPPORT_CANCEL_GRACE_TIME = 0.3f;

        private bool delayedBlockStop = false;
        private float cancelTimer = -1f;

        protected override float SpecificCardRenewCooldown()
        {
            return GameConstantsBehaviour.Instance.bodyCardRenewCooldown.GetValue();
        }

        public PlayerBodyAbilityInput(Player player, BattleStatusManager battleStatus) : base(player, battleStatus)
        {
            battleStatus.OnStatusApplied += OnStatusApplied;
            battleStatus.health.OnAttackReceived += AttackReceived;
        }

        protected override void Tick(float delta)
        {
            if (cancelTimer > 0f)
            {
                cancelTimer -= delta;
                if (cancelTimer <= 0f)
                {
                    if (((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).HaveBlock())
                    {
                        ((BodyBlockStatusEffect)battleStatus.GetStatus(StatusType.BodyBlockStatus)).CancelBlock();
                    }
                    else
                    {
                        delayedBlockStop = true;
                    }
                }
            }
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryToExecuteAbility(5);
                cancelTimer = -1f;
            }
            if (context.canceled)
            {
                cancelTimer = SUPPORT_CANCEL_GRACE_TIME;
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
            } else if (statusType == StatusIconType.Rush)
            {
                RenewAllEmptyAbilities();
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
                //renewTimer = 0f;
                RenewAllEmptyAbilities();
            }
        }
    }
}