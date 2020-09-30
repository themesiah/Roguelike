using UnityEngine;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    public abstract class CharacterBattleBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected RuntimeBattleBehaviourSet selfBattleBehaviour = default;
        [SerializeField]
        protected RuntimeBattleBehaviourSet enemiesBattleBehaviour = default;

        public CharacterHealth CharacterHealth { get; protected set; }
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }


        protected virtual void Start()
        {
            SetupStatusManager();
            SetupAbilityInputProcessor();
            SetupAbilityInputExecutor();
        }

        protected abstract void SetupStatusManager();
        protected abstract void SetupAbilityInputProcessor();
        protected abstract void SetupAbilityInputExecutor();

        protected virtual void OnEnable()
        {
            selfBattleBehaviour.Add(this);
        }

        protected virtual void OnDisable()
        {
            selfBattleBehaviour.Remove(this);
        }

        protected virtual void Update()
        {
            StatusManager.ProcessStatus(Time.deltaTime);
            int index = AbilityInputProcessor.GetAbilityToExecute(StatusManager, Time.deltaTime);
            if (index != -1)
            {
                var statuses = GetStatuses();
                AbilityExecutor.ExecuteAbility(index, StatusManager, statuses);
            }
        }

        private BattleStatusManager[] GetStatuses()
        {
            var enemies = GetEnemies();
            BattleStatusManager[] statuses = new BattleStatusManager[enemies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = enemies[i].StatusManager;
            }
            return statuses;
        }

        protected CharacterBattleBehaviour[] GetEnemies() // First is selected enemy.
        {
            return enemiesBattleBehaviour.Items.ToArray();
        }
    }
}