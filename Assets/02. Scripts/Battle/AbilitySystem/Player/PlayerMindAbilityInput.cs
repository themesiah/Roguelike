using Laresistance.Behaviours;
using Laresistance.Core;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public class PlayerMindAbilityInput : PlayerAbilityInput
    {
        protected override float SpecificCardRenewCooldown()
        {
            return GameConstantsBehaviour.Instance.mindCardRenewCooldown.GetValue();
        }

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
            if (context.performed) TryToExecuteAbility(5);
        }

        protected override void OnBattleStart()
        {
        }

        protected override void OnBattleEnd()
        {
        }

        private void Shuffle(BattleStatusManager bsm)
        {
            renewTimer = TotalCardRenewCooldown;
            ExecuteOnNextCardProgress();
            int[] discarded = DiscardAvailableAbilities();
            ExecuteOnShuffle(discarded);
            RenewAllAbilities();
            ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
            // Si haces shuffle, debería cargarse las habilidades en cola porque no debería poder hacerlas
            ClearAbilityToUseQueue();
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        protected override void OnInitializeAbilities()
        {
            player.supportAbility.SetCooldownAsUsed();
        }

        protected override void OnAbilitiesUpdate(float delta, float unmodifiedDelta)
        {
            player.supportAbility?.Tick(unmodifiedDelta);
            ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
        }

        protected override void OnAbilityExecutedExtra(BattleAbility ability, int slot)
        {
            battleStatus.AddEnergy(GameConstantsBehaviour.Instance.mindEnergyGain.GetValue());
        }

        protected override bool CanExecuteAbilities()
        {
            // No deberia poder hacer habilidades si shuffle está en la cola del manager
            if (BattleAbilityManager.Instance.IsAbilityInQueue(player.supportAbility) || nextAbilitiesQueue.Contains(player.supportAbility))
            {
                return false;
            }
            return true;
        }
    }
}