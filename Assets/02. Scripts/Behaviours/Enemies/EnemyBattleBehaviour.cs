using Laresistance.Battle;
using Laresistance.Data;
using UnityEngine;
using UnityEngine.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

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
        [SerializeField]
        protected ScriptableVector2Reference levelVarianceRef = default;
        [SerializeField]
        protected Transform effectTargetPivot = default;
        [SerializeField]
        protected UnityEvent<string> OnEnemyName;
        [SerializeField]
        protected UnityEvent<int> OnEnemyLevel;

        protected int enemyLevel;

        protected void GenerateEnemyLevel()
        {
            enemyLevel = System.Math.Max(1, (int)((currentLevel.GetValue() * levelVarianceRef.GetValue().x) + Random.Range(-levelVarianceRef.GetValue().y, levelVarianceRef.GetValue().y)));
            OnEnemyLevel?.Invoke(enemyLevel);
        }

        protected override void SetupStatusManager()
        {
            GenerateEnemyLevel();
            StatusManager = new BattleStatusManager(new CharacterHealth(enemyData.MaxHealth * (int)(1f + (enemyLevel - 1) * 0.1f)), effectTargetPivot);
            OnEnemyName?.Invoke(Texts.GetText(enemyData.NameRef));
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

            AbilityInputProcessor = new EnemyAbilityManager(abilities, enemyLevel, animator);
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
            int bloodToGet = enemyData.BaseBloodReward * enemyLevel;
            playerDataRef.Get().player.GetEquipmentEvents().OnGetExtraBlood?.Invoke(ref bloodToGet);
            RewardData rewardData = new RewardData(bloodToGet, 0, null, null, null, null);
            return rewardData;
        }

        public override BattleAbility[] GetAbilities()
        {
            return AbilityInputProcessor.GetAbilities();
        }

        public override BattleAbility[] GetAllAbilities()
        {
            return AbilityInputProcessor.GetAbilities();
        }
    }
}