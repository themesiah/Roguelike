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

        [SerializeField]
        private List<MinionData> startingMinions = default;
        [SerializeField]
        private AnimatorWrapperBehaviour animatorReference = default;

        private BattleStatusManager targetStatus = null;

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new PlayerAbilityInput(player);
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = new PlayerAbilityExecutor(player, animator);
        }

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(100));
        }

        protected override void ConfigurePrefab()
        {

        }

        protected override void Awake()
        {
            player = new Player();
            ///////
            //List<BattleEffect> testEffects = new List<BattleEffect>();
            //testEffects.Add(new BattleEffectDamage(50, EffectTargetType.Enemy));
            //BattleAbility testAbility = new BattleAbility(testEffects, 3f, player.GetEquipmentEvents());
            //Minion m = new Minion("", testAbility, 1);
            //player.EquipMinion(m);
            //
            //List<BattleEffect> testEffects2 = new List<BattleEffect>();
            //testEffects2.Add(new BattleEffectHeal(5, EffectTargetType.Self));
            //BattleAbility testAbility2 = new BattleAbility(testEffects2, 4f, player.GetEquipmentEvents());
            //Minion m2 = new Minion("", testAbility2, 1);
            //player.EquipMinion(m2);
            ///////
            SetAnimator(animatorReference);
            base.Awake();
            

            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(15, EffectTargetType.Enemy, StatusManager));
            BattleAbility testAbility = new BattleAbility(testEffects, 3f, player.GetEquipmentEvents());
            player.SetMainAbility(testAbility);

            foreach (var md in startingMinions)
            {
                Minion m = MinionFactory.GetMinion(md, 1, player.GetEquipmentEvents(), StatusManager);
                player.EquipMinion(m);
            }
        }

        public void PerformPlayerAction(InputAction.CallbackContext context) => PerformAbility(context, 0);
        public void PerformMinionAbility1(InputAction.CallbackContext context) => PerformAbility(context, 1);
        public void PerformMinionAbility2(InputAction.CallbackContext context) => PerformAbility(context, 2);
        public void PerformMinionAbility3(InputAction.CallbackContext context) => PerformAbility(context, 3);
        public void PerformConsumableAbility1(InputAction.CallbackContext context) => PerformAbility(context, 4);
        public void PerformConsumableAbility2(InputAction.CallbackContext context) => PerformAbility(context, 5);
        public void PerformConsumableAbility3(InputAction.CallbackContext context) => PerformAbility(context, 6);
        public void PerformChangeTarget(InputAction.CallbackContext context)
        {

        }

        public void PerformAbility(InputAction.CallbackContext context, int abilityIndex)
        {
            if (context.performed) ((PlayerAbilityInput)AbilityInputProcessor).TryToExecuteAbility(abilityIndex);
        }

        public void ResetAbilities()
        {
            player.ResetAbilities();
            StatusManager.ResetModifiers();
        }

        public void ChangeTarget(int index)
        {
            targetStatus = enemiesBattleBehaviour.Items[index].StatusManager;
        }

        public void ChangeTargetToFirstNotDead()
        {
            if (targetStatus != null && targetStatus.health.GetCurrentHealth() > 0)
                return;
            for (int i = 0; i < enemiesBattleBehaviour.Items.Count; ++i)
            {
                if (enemiesBattleBehaviour.Items[i].StatusManager.health.GetCurrentHealth() > 0)
                {
                    ChangeTarget(i);
                    break;
                }
            }
        }

        protected override BattleStatusManager[] GetStatuses()
        {
            var enemies = GetEnemies();
            BattleStatusManager[] statuses = new BattleStatusManager[enemies.Length];
            int targetIndex = 0;
            for (int i = 0; i < statuses.Length; ++i)
            {
                if (targetStatus == enemies[i].StatusManager)
                {
                    targetIndex = i;
                    break;
                }
            }

            if (targetIndex != 0)
            {
                var temp = enemies[0];
                enemies[0] = enemies[targetIndex];
                enemies[targetIndex] = temp;
            }

            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = enemies[i].StatusManager;
            }

            return statuses;
        }
    }
}