using Laresistance.Battle;
using Laresistance.Data;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public class EnemyMinionBattleBehaviour : EnemyBattleBehaviour
    {
        private Minion minion;

        protected override void SetupStatusManager()
        {
            base.SetupStatusManager();
            minion = MinionFactory.GetMinion((MinionData)enemyData, enemyLevel, new EquipmentsContainer(), StatusManager);
        }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new EnemyAbilityManager(minion.Abilities, minion.Level);
            if (animator != null)
            {
                ((EnemyAbilityManager)AbilityInputProcessor).SetAnimator(animator);
            }
        }

        public override RewardData GetReward()
        {
            RewardData rewardData = new RewardData(0, 0, minion, null, null, null, null);
            return rewardData;
        }

        protected override void ConfigurePrefab()
        {
            EnemyPrefabConfiguration epc = GetComponent<EnemyPrefabConfiguration>();
            if (epc != null)
            {
                epc.ConfigurePrefab(enemyData.PrefabReference, (anim) =>
                {
                    SetAnimator(anim);
                    if (AbilityInputProcessor != null)
                        ((EnemyAbilityManager)AbilityInputProcessor).SetAnimator(anim);
                });
            }
        }

        public Minion Minion => minion;
    }
}