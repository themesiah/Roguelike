using Laresistance.Battle;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    public class PlayerBattleBehaviour : CharacterBattleBehaviour
    {
        private Player player;

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = new PlayerAbilityExecutor(player);
        }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new PlayerAbilityInput(player);
        }

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(100));
        }

        protected override void Start()
        {
            player = new Player();
            ///////
            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(10));
            BattleAbility testAbility = new BattleAbility(testEffects, 3f, player.GetEquipmentEvents());
            Minion m = new Minion("", testAbility, 1);
            player.EquipMinion(m);
            ///////

            base.Start();
        }
    }
}