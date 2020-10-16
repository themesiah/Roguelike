﻿using Laresistance.Battle;
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
            minion = MinionFactory.GetMinion((MinionData)enemyData, currentLevel.GetValue(), null, StatusManager);
        }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new EnemyAbilityManager(minion.Abilities, minion.Level, animator);
        }

        public override RewardData GetReward()
        {
            RewardData rewardData = new RewardData(0, minion, null);
            return rewardData;
        }

        protected override void ConfigurePrefab()
        {
            EnemyPrefabConfiguration epc = GetComponent<EnemyPrefabConfiguration>();
            if (epc != null)
            {
                epc.ConfigurePrefab(enemyData.Prefab, SetAnimator);
            }
        }

        public Minion Minion => minion;
    }
}