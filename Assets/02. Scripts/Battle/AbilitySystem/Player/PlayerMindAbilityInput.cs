using Laresistance.Behaviours;
using Laresistance.Core;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public class PlayerMindAbilityInput : PlayerAbilityInput
    {

        public override float TotalSupportAbilityCooldown
        {
            get
            {
                float total = player.supportAbility.Cooldown;
                total = player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.ShuffleDelay, total);
                return total;
            }
        }

        public PlayerMindAbilityInput(Player player, BattleStatusManager battleStatus) : base(player, battleStatus)
        {
            battleStatus.OnSupportAbilityExecuted += Shuffle;
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
            //if (context.performed) Shuffle(battleStatus);
            if (context.performed) TryToExecuteAbility(5);
        }

        protected override void AfterUpdateAbilities(float delta)
        {
        }

        protected override void OnBattleStart()
        {
        }

        private void Shuffle(BattleStatusManager bsm)
        {
            //if (!BattleAbilityManager.Instance.Executing && !BattleAbilityManager.Instance.AbilityInQueue && supportAbilityTimer <= 0f && battleStatus.Stunned == false && abilitiesToUseList.Count == 0)
            {
                renewTimer = TotalCardRenewCooldown;
                ExecuteOnNextCardProgress();
                int[] discarded = DiscardAvailableAbilities();
                ExecuteOnShuffle(discarded);
                //float energyValue = discarded.Length;
                //energyValue = player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.ShuffleEnergyGain, energyValue);
                //battleStatus.AddEnergy(energyValue);
                RenewAllAbilities();
                ExecuteOnNextShuffleProgress(NextSupportAbilityProgress);
            }
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        protected override void OnInitializeAbilities()
        {
            player.supportAbility.SetCooldownAsUsed();
        }

        protected override void OnAbilitiesUpdate(float delta)
        {
            player.supportAbility?.Tick(delta);
            ExecuteOnNextShuffleProgress(NextSupportAbilityProgress);
        }

        protected override void OnAbilityExecutedExtra(BattleAbility ability, int slot)
        {
            battleStatus.AddEnergy(0.5f);
        }
    }
}