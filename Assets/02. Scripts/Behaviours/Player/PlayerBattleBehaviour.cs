using Laresistance.Battle;
using UnityEngine;
using UnityEngine.InputSystem;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class PlayerBattleBehaviour : CharacterBattleBehaviour
    {
        [SerializeField]
        private PlayerDataBehaviour playerDataBehaviour = default;
        [SerializeField]
        private AnimatorWrapperBehaviour animatorReference = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new PlayerAbilityInput(playerDataBehaviour.player, StatusManager);
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = new PlayerAbilityExecutor(playerDataBehaviour.player, animator, bloodReference);
        }

        protected override void SetupStatusManager()
        {
            StatusManager = playerDataBehaviour.StatusManager;
        }

        protected override void SetupTargetSelector()
        {
            TargetSelector = new PlayerTargetSelection();
        }

        protected override void ConfigurePrefab()
        {

        }

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            SetAnimator(animatorReference);
            base.Init();
        }

        public void PerformPlayerAction(InputAction.CallbackContext context) => PerformAbility(context, 0);
        public void PerformMinionAbility1(InputAction.CallbackContext context) => PerformAbility(context, 1);
        public void PerformMinionAbility2(InputAction.CallbackContext context) => PerformAbility(context, 2);
        public void PerformMinionAbility3(InputAction.CallbackContext context) => PerformAbility(context, 3);
        public void PerformUltimateAbility(InputAction.CallbackContext context) => PerformAbility(context, 4);
        public void PerformReshuffle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ((PlayerAbilityInput)AbilityInputProcessor).Shuffle();
            }
        }

        public void PerformChangeTarget(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                float val = context.ReadValue<float>();
                ((PlayerTargetSelection)TargetSelector).DoTargetSelection(val > 0f ? true : false);
            }
        }

        public void PerformAbility(InputAction.CallbackContext context, int abilityIndex)
        {
            if (context.performed) ((PlayerAbilityInput)AbilityInputProcessor).TryToExecuteAbility(abilityIndex);
        }

        public override BattleAbility[] GetAbilities()
        {
            return playerDataBehaviour.player.GetAbilities();
        }

        public override BattleAbility[] GetAllAbilities()
        {
            return playerDataBehaviour.player.GetAllAbilities();
        }

        public override void OutsideBattleHeal(int heal)
        {
            base.OutsideBattleHeal(heal);
        }
    }
}