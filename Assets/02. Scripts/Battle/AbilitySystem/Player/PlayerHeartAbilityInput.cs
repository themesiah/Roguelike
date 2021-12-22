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

        private void OnHealed(CharacterHealth sender, int healAmount, int currentHealth)
        {
            float energy = healAmount * GameConstantsBehaviour.Instance.heartEnergyGain.GetValue();
            energy = player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.EnergyGain, energy);
            battleStatus.AddEnergy(energy);
        }

        public override void SupportAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(5);
        }

        protected override void OnBattleStart()
        {
            battleStatus.health.OnHealed += OnHealed;
        }

        protected override void OnBattleEnd()
        {
            battleStatus.health.OnHealed -= OnHealed;
        }

        public override void UltimateAbility(InputAction.CallbackContext context)
        {
            if (context.performed) TryToExecuteAbility(4);
        }

        protected override bool CanExecuteAbilities()
        {
            return true;
        }

        protected override void OnInitializeAbilities()
        {
            player.supportAbility.SetCooldownAsUsed();
        }

        protected override void OnAbilitiesUpdate(float delta, float unmodifiedDelta)
        {
            if (!battleStatus.GetStatus(StatusType.Vampirism).HaveBuff())
            {
                float newDelta = player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.SupportRenewSpeed, unmodifiedDelta);
                player.supportAbility?.Tick(newDelta);
            }
            ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
        }
    }
}