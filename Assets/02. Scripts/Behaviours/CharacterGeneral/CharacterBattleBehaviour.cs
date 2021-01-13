using UnityEngine;
using Laresistance.Battle;
using UnityEngine.Events;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Sets;

namespace Laresistance.Behaviours
{
    public abstract class CharacterBattleBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected UnityEvent OnBattleBehaviourEnable = default;
        [SerializeField]
        protected UnityEvent OnBattleBehaviourDisable = default;
        [SerializeField]
        protected RuntimeSingleGameObject selectionArrowReference = default;
        [SerializeField]
        protected RuntimeSingleGameObject selectionArrowExtendedInfoReference = default;
        [SerializeField]
        protected Transform selectionArrowPivot = default;
        [SerializeField]
        protected ExtendedBattleInfo extendedBattleInfo = default;
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }
        public ITargetSelection TargetSelector { get; protected set; }
        public UnityAction OnStatusGenerated = delegate { };
        protected IBattleAnimator animator;
        public CharacterBattleManager battleManager { get; protected set; }

        protected virtual void Awake()
        {
            ConfigurePrefab();
            SetupStatusManager();
            SetupAbilityInputProcessor();
            SetupAbilityInputExecutor();
            SetupTargetSelector();
            OnStatusGenerated.Invoke();
            battleManager = new CharacterBattleManager(StatusManager, AbilityInputProcessor, AbilityExecutor, TargetSelector, animator);
            battleManager.OnBattleStart += OnBattleBehaviourEnable.Invoke;
            battleManager.OnBattleEnd += OnBattleBehaviourDisable.Invoke;
            battleManager.OnSelected += SelectionArrowOnPivot;
            this.enabled = false;
        }

        protected abstract void SetupStatusManager();
        protected abstract void SetupAbilityInputProcessor();
        protected abstract void SetupAbilityInputExecutor();
        protected abstract void SetupTargetSelector();
        protected abstract void ConfigurePrefab();
        protected void SetAnimator(IBattleAnimator animator)
        {
            this.animator = animator;
        }

        private void SelectionArrowOnPivot(bool selected)
        {
            Transform t = selectionArrowReference.Get().transform;
            if (selected == true)
            {
                t.SetParent(selectionArrowPivot);
                t.localPosition = Vector3.zero;
            } else
            {
                t.SetParent(null);
            }
            t.GetChild(0).gameObject.SetActive(selected);

            GameObject extendedInfoArrow = selectionArrowExtendedInfoReference.Get();
            if (selected == true)
            {
                extendedBattleInfo.SetSelectionArrow(extendedInfoArrow);
            } else
            {
                extendedInfoArrow.transform.SetParent(null);
            }
            extendedInfoArrow.SetActive(selected);
        }

        public void OutsideBattleHeal(int heal)
        {
            StatusManager.health.Heal(heal);
        }

        public void OutsideBattleDamage(int damage)
        {
            StatusManager.health.TakeDamage(damage, null);
        }

        public abstract BattleAbility[] GetAbilities();
        public abstract BattleAbility[] GetAbilitiesWithUltimate();
    }
}