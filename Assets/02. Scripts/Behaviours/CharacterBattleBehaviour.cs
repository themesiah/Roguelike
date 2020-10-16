using UnityEngine;
using Laresistance.Battle;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using UnityEngine.Events;
using System;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public abstract class CharacterBattleBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected RuntimeBattleBehaviourSet selfBattleBehaviour = default;
        [SerializeField]
        protected RuntimeBattleBehaviourSet enemiesBattleBehaviour = default;
        [SerializeField]
        protected RuntimeBattleBehaviourSet alliesBattleBehaviour = default;
        [SerializeField]
        protected UnityEvent OnBattleBehaviourEnable = default;
        [SerializeField]
        protected UnityEvent OnBattleBehaviourDisable = default;
        
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }

        public UnityAction OnStatusGenerated = delegate { };

        protected IBattleAnimator animator;
        public CharacterBattleManager battleManager { get; protected set; }


        protected virtual void Awake()
        {
            ConfigurePrefab();
            SetupStatusManager();
            SetupAbilityInputProcessor();
            SetupAbilityInputExecutor();
            OnStatusGenerated.Invoke();
            battleManager = new CharacterBattleManager(StatusManager, AbilityInputProcessor, AbilityExecutor, animator);
            battleManager.OnBattleStart += OnBattleBehaviourEnable.Invoke;
            battleManager.OnBattleEnd += OnBattleBehaviourDisable.Invoke;
            this.enabled = false;
        }

        protected abstract void SetupStatusManager();
        protected abstract void SetupAbilityInputProcessor();
        protected abstract void SetupAbilityInputExecutor();
        protected abstract void ConfigurePrefab();
        protected void SetAnimator(IBattleAnimator animator)
        {
            this.animator = animator;
        }

        //protected virtual void OnEnable()
        //{
        //    selfBattleBehaviour.Add(this);
        //    OnBattleBehaviourEnable?.Invoke();
        //}
        //
        //protected virtual void OnDisable()
        //{
        //    selfBattleBehaviour.Remove(this);
        //    OnBattleBehaviourDisable?.Invoke();
        //}

        protected virtual void Update()
        {
            //battleManager.Tick(Time.deltaTime);
            //
            //StatusManager.ProcessStatus(Time.deltaTime);
            //int index = AbilityInputProcessor.GetAbilityToExecute(StatusManager, Time.deltaTime);
            //if (index != -1)
            //{
            //    var statuses = GetStatuses();
            //    var allyStatuses = GetAllyStatuses();
            //    StartCoroutine(AbilityExecutor.ExecuteAbility(index, allyStatuses, statuses));
            //}
        }

        protected virtual BattleStatusManager[] GetStatuses()
        {
            var enemies = GetEnemies();
            BattleStatusManager[] statuses = new BattleStatusManager[enemies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = enemies[i].StatusManager;
            }
            return statuses;
        }

        protected CharacterBattleBehaviour[] GetEnemies()
        {
            return enemiesBattleBehaviour.Items.ToArray();
        }

        protected CharacterBattleBehaviour[] GetAllies()
        {
            return alliesBattleBehaviour.Items.ToArray();
        }

        private BattleStatusManager[] GetAllyStatuses()
        {
            var allies = GetAllies();
            BattleStatusManager[] statuses = new BattleStatusManager[allies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = allies[i].StatusManager;
            }
            if (statuses[0] != this.StatusManager)
            {
                var temp = statuses[0];
                statuses[0] = this.StatusManager;
                for (int i = 0; i < statuses.Length; ++i)
                {
                    if (statuses[i] == this.StatusManager)
                    {
                        statuses[i] = temp;
                    }
                }
            }
            return statuses;
        }
    }
}