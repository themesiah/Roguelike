using Laresistance.Battle;
using System.Collections.Generic;

namespace Laresistance.Behaviours
{
    public class EnemyBattleBehaviour : CharacterBattleBehaviour
    {
        protected override BattleStatusManager[] GetEnemies()
        {
            return new BattleStatusManager[] { StatusManager };
        }

        protected override void Start()
        {
            base.Start();

            List<BattleEffect> testEffects = new List<BattleEffect>();
            testEffects.Add(new BattleEffectDamage(10));
            BattleAbility[] testAbilities = new BattleAbility[] { new BattleAbility(testEffects, 3f) };

            AbilityInputProcessor = new EnemyAbilityManager(testAbilities, 1);
            AbilityExecutor = (IAbilityExecutor)AbilityInputProcessor;
        }
    }
}