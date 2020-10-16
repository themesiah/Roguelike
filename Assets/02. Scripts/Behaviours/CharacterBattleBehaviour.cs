using UnityEngine;
using Laresistance.Battle;
using UnityEngine.Events;
using Laresistance.Core;

namespace Laresistance.Behaviours
{
    public abstract class CharacterBattleBehaviour : MonoBehaviour
    {
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
    }
}