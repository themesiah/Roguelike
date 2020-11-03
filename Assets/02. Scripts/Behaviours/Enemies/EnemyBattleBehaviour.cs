using Laresistance.Battle;
using Laresistance.Data;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class EnemyBattleBehaviour : CharacterBattleBehaviour, IRewardable
    {
        [SerializeField]
        protected EnemyData enemyData = default;
        [SerializeField]
        protected ScriptableIntReference currentLevel = default;
        [SerializeField]
        protected RuntimePlayerDataBehaviourSingle playerDataRef = default;

        protected override void SetupStatusManager()
        {
            StatusManager = new BattleStatusManager(new CharacterHealth(enemyData.MaxHealth * (int)(1f + (currentLevel.GetValue() - 1) * 0.1f)));
        }

        protected override void SetupAbilityInputExecutor()
        {
            AbilityExecutor = (IAbilityExecutor)AbilityInputProcessor;
        }

        protected override void SetupAbilityInputProcessor()
        {
            BattleAbility[] abilities = new BattleAbility[enemyData.AbilitiesData.Length];
            for(int i = 0; i < enemyData.AbilitiesData.Length; ++i)
            {
                abilities[i] = BattleAbilityFactory.GetBattleAbility(enemyData.AbilitiesData[i], null, StatusManager);
            }

            AbilityInputProcessor = new EnemyAbilityManager(abilities, currentLevel.GetValue(), animator);
        }

        protected override void ConfigurePrefab()
        {
            EnemyPrefabConfiguration epc = GetComponent<EnemyPrefabConfiguration>();
            if (epc != null)
            {
                epc.ConfigurePrefab(enemyData.Prefab, SetAnimator);
            }
        }

        protected override void SetupTargetSelector()
        {
            TargetSelector = new DummyTargetSelection();
        }

        public virtual RewardData GetReward()
        {
            int bloodToGet = enemyData.BaseBloodReward * currentLevel.GetValue();
            playerDataRef.Get().player.GetEquipmentEvents().OnGetExtraBlood?.Invoke(ref bloodToGet);
            RewardData rewardData = new RewardData(bloodToGet, 0, null, null, null, null);
            return rewardData;
        }
    }
}