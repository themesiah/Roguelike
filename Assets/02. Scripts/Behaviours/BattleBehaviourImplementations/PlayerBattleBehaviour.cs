using Laresistance.Battle;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;
using Laresistance.Data;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class PlayerBattleBehaviour : CharacterBattleBehaviour
    {
        public Player player { get; private set; }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new PlayerAbilityInput(player);
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = new PlayerAbilityExecutor(player);
        }

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(100));
        }

        protected override void Awake()
        {
            player = new Player();
            ///////
            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(20, EffectTargetType.Enemy));
            BattleAbility testAbility = new BattleAbility(testEffects, 3f, player.GetEquipmentEvents());
            Minion m = new Minion("", testAbility, 1);
            player.EquipMinion(m);

            List<BattleEffect> testEffects2 = new List<BattleEffect>();
            testEffects2.Add(new BattleEffectHeal(5, EffectTargetType.Self));
            BattleAbility testAbility2 = new BattleAbility(testEffects2, 4f, player.GetEquipmentEvents());
            Minion m2 = new Minion("", testAbility2, 1);
            player.EquipMinion(m2);
            ///////

            base.Awake();
        }

        public void PerformPlayerAction(InputAction.CallbackContext context) => PerformAbility(context, 0);
        public void PerformMinionAbility1(InputAction.CallbackContext context) => PerformAbility(context, 1);
        public void PerformMinionAbility2(InputAction.CallbackContext context) => PerformAbility(context, 2);
        public void PerformMinionAbility3(InputAction.CallbackContext context) => PerformAbility(context, 3);
        public void PerformConsumableAbility1(InputAction.CallbackContext context) => PerformAbility(context, 4);
        public void PerformConsumableAbility2(InputAction.CallbackContext context) => PerformAbility(context, 5);
        public void PerformConsumableAbility3(InputAction.CallbackContext context) => PerformAbility(context, 6);

        public void PerformAbility(InputAction.CallbackContext context, int abilityIndex)
        {
            if (context.performed) ((PlayerAbilityInput)AbilityInputProcessor).TryToExecuteAbility(abilityIndex);
        }
    }
}