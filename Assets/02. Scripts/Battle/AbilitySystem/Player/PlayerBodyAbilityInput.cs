using Laresistance.Behaviours;
using Laresistance.Core;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Laresistance.Battle
{
    public class PlayerBodyAbilityInput : PlayerAbilityInput
    {
        public override float TotalSupportAbilityCooldown
        {
            get
            {
                float total = player.supportAbility.Cooldown;
                return total;
            }
        }

        protected override float SpecificCardRenewCooldown()
        {
            return GameConstantsBehaviour.Instance.bodyCardRenewCooldown.GetValue();
        }

        public PlayerBodyAbilityInput(Player player, BattleStatusManager battleStatus) : base(player, battleStatus)
        {
            battleStatus.OnStatusApplied += OnStatusApplied;
            battleStatus.OnSupportAbilityExecuted += OnBlockExecuted;
        }

        private void OnBlockExecuted(BattleStatusManager sender)
        {
            ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryToExecuteAbility(5);
            }
        }

        protected override void OnBattleStart()
        {
            ExecuteOnNextSupportAbilityProgress(1f);
            battleStatus.health.OnShieldsChanged += OnBlockDamage;
            battleStatus.health.OnPercentBlocked += OnPercentBlockDamage;
        }

        protected override void OnBattleEnd()
        {
            battleStatus.health.OnShieldsChanged -= OnBlockDamage;
            battleStatus.health.OnPercentBlocked -= OnPercentBlockDamage;
        }

        private void OnBlockDamage(CharacterHealth sender, int delta, int totalShields, bool isDamage, float healthShieldPercent)
        {
            if (isDamage && delta < 0)
            {
                battleStatus.AddEnergy(Mathf.Abs(delta) * GameConstantsBehaviour.Instance.bodyEnergyGain.GetValue());
            }
        }

        private void OnPercentBlockDamage(CharacterHealth sender, float percentBlocked, int damageBlocked)
        {
            if (percentBlocked > 0f)
            {
                battleStatus.AddEnergy(damageBlocked * GameConstantsBehaviour.Instance.bodyEnergyGain.GetValue());
            }
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        protected override void OnInitializeAbilities()
        {
            player.supportAbility.ResetCooldown();
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
            return true;
        }

        protected override void OnAbilitiesUpdate(float delta, float unmodifiedDelta)
        {
            player.supportAbility?.Tick(unmodifiedDelta);
            ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
        }

        protected override void OnAbilityExecutedExtra(BattleAbility ability, int slot)
        {
            if ((((RushStatusEffect)battleStatus.GetStatus(StatusType.Rush)).HaveRush()))
            {
                RenewAllEmptyAbilities();
            }
        }
    }
}