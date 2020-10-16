using Laresistance.Battle;
using System.Collections.Generic;
using Laresistance.Data;
using Laresistance.Core;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class EnemyMinionBattleBehaviour : CharacterBattleBehaviour, IRewardable
    {
        [SerializeField]
        private MinionData minionData = default;

        [SerializeField]
        private ScriptableIntReference currentLevel = default;

        private Minion minion;

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(minionData.MaxHealth * (int)(1f + (currentLevel.GetValue() - 1) * 0.1f)));
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = (IAbilityExecutor)AbilityInputProcessor;
        }

        protected override void SetupAbilityInputProcessor()
        {
            AbilityInputProcessor = new EnemyAbilityManager(minion.Abilities, minion.Level, animator);
        }

        protected override void Awake()
        {
            ConfigurePrefab();
            SetupStatusManager();
            minion = MinionFactory.GetMinion(minionData, currentLevel.GetValue(), null, StatusManager);
            SetupAbilityInputProcessor();
            SetupAbilityInputExecutor();
            OnStatusGenerated.Invoke();
            battleManager = new CharacterBattleManager(StatusManager, AbilityInputProcessor, AbilityExecutor, animator);
            battleManager.OnBattleStart += OnBattleBehaviourEnable.Invoke;
            battleManager.OnBattleEnd += OnBattleBehaviourDisable.Invoke;
            this.enabled = false;
        }

        public RewardData GetReward()
        {
            RewardData rewardData = new RewardData(0, minion, null);
            return rewardData;
        }

        protected override void ConfigurePrefab()
        {
            EnemyPrefabConfiguration epc = GetComponent<EnemyPrefabConfiguration>();
            if (epc != null)
            {
                epc.ConfigurePrefab(minionData.Prefab, SetAnimator);
            }
        }

        public Minion Minion => minion;
    }
}