using Laresistance.Battle;
using Laresistance.Data;
using Laresistance.Core;
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
        [SerializeField]
        protected bool partyMember = false;
        [SerializeField]
        protected int enemyLevel = 0;

        public void InitEnemy(int overrideLevel)
        {
            if (overrideLevel > 0)
            {
                enemyLevel = overrideLevel;
            }
            Init();
            PartyManagerBehaviour enemyParty = GetComponent<PartyManagerBehaviour>();
            if (enemyParty != null && enemyParty.enabled == true)
            {
                enemyParty.SpawnParty(enemyLevel);
            }
        }

        protected void GenerateEnemyLevel()
        {
            if (enemyLevel == 0)
            {
                enemyLevel = System.Math.Max(1, (int)((currentLevel.GetValue() * levelVarianceRef.GetValue().x) + Random.Range(-levelVarianceRef.GetValue().y, levelVarianceRef.GetValue().y)));
            }
            OnEnemyLevel?.Invoke(enemyLevel);
        }

        protected override void SetupStatusManager()
        {
            GenerateEnemyLevel();
            StatusManager = new BattleStatusManager(new CharacterHealth((int)(enemyData.MaxHealth * (1f + (enemyLevel - 1) * 0.1f))), effectTargetPivot);
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
                abilities[i] = BattleAbilityFactory.GetBattleAbility(enemyData.AbilitiesData[i], new EquipmentsContainer(), StatusManager);
            }

            AbilityInputProcessor = new EnemyAbilityManager(abilities, enemyLevel);
            if (animator != null)
            {
                ((EnemyAbilityManager)AbilityInputProcessor).SetAnimator(animator);
            }
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
                }, partyMember);
            }
        }

        protected override void SetupTargetSelector()
        {
            TargetSelector = new DummyTargetSelection();
        }

        public virtual RewardData GetReward()
        {
            int bloodToGet = enemyData.BaseBloodReward * enemyLevel;
            bloodToGet = playerDataRef.Get().player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.ExtraBlood, bloodToGet);
            RewardData rewardData = new RewardData(bloodToGet, 0, null, null, null, null, null);
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