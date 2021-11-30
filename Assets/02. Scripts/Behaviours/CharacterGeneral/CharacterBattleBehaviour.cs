using UnityEngine;
using Laresistance.Battle;
using UnityEngine.Events;
using Laresistance.Core;
using GamedevsToolbox.ScriptableArchitecture.Sets;
using UnityEngine.Analytics;
using System.Collections.Generic;
using Laresistance.Systems;

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
        [SerializeField]
        protected AnimatorWrapperBehaviour animator;

        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }
        public ITargetSelection TargetSelector { get; protected set; }
        public UnityAction OnStatusGenerated = delegate { };
        public CharacterBattleManager battleManager { get; protected set; }

        public virtual void Init()
        {
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
                extendedBattleInfo.Selected();
            } else
            {
                extendedInfoArrow.transform.SetParent(null);
                extendedBattleInfo.Unselected();
            }
            extendedInfoArrow.SetActive(selected);
        }

        public virtual void OutsideBattleHeal(int heal)
        {
            int finalHeal = StatusManager.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.FountainHealing, heal);
            StatusManager.health.Heal(finalHeal);
            AnalyticsSystem.Instance.CustomEvent("SanctuaryHeal", new Dictionary<string, object>() {
                { "Amount healed", heal }
            });
        }

        protected virtual void OutsideBattleDamage(int damage)
        {
            StatusManager.health.TakeDamage(damage, new EquipmentsContainer(), new EquipmentsContainer());
        }

        public void FallDamage(int damage)
        {
            AnalyticsSystem.Instance.CustomEvent("TrapDamage", new Dictionary<string, object>() {
                { "Type", "Fall" },
                { "DamageReceived", damage }
            });
            OutsideBattleDamage(damage);
        }

        public abstract BattleAbility[] GetAbilities();
        public abstract BattleAbility[] GetAllAbilities();
    }
}