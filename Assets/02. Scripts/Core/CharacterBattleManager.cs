using Laresistance.Battle;
using Laresistance.Behaviours;
using Laresistance.Systems;
using System.Collections;
using System.Collections.Generic;

namespace Laresistance.Core
{
    public class CharacterBattleManager : ITimeStoppable, ITargetDependant
    {
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }
        public ITargetSelection TargetSelector { get; protected set; }

        public bool selected { get; protected set; }
        protected IBattleAnimator animator;

        private CharacterBattleManager[] allies;
        private CharacterBattleManager[] enemies;
        private BattleSystem battleSystem;
        private List<int> abilitiesToUse;
        private CharacterBattleManager selectedTarget = null;

        public delegate void OnBattleStartHandler();
        public event OnBattleStartHandler OnBattleStart;
        public delegate void OnBattleEndHandler();
        public event OnBattleEndHandler OnBattleEnd;
        public delegate void OnSelectedHandler(bool selected);
        public event OnSelectedHandler OnSelected;

        public bool dead { get; private set; }
        public CharacterBattleManager[] Enemies => enemies;

        public CharacterBattleManager(BattleStatusManager statusManager, IAbilityInputProcessor inputProcessor, IAbilityExecutor abilityExecutor, ITargetSelection targetSelector, IBattleAnimator animator)
        {
            StatusManager = statusManager;
            AbilityInputProcessor = inputProcessor;
            AbilityExecutor = abilityExecutor;
            TargetSelector = targetSelector;
            this.animator = animator;
            selected = false;
            dead = false;
            abilitiesToUse = new List<int>();
            StatusManager.SetCharacterBattleManager(this);
        }

        public void SetAnimator(IBattleAnimator animator)
        {
            this.animator = animator;
        }

        public void StartBattle()
        {
            OnBattleStart?.Invoke();
            StatusManager.BattleStart();
            AbilityInputProcessor.BattleStart();
            animator.SetTrigger("StartBattle");
        }

        public void EndBattle()
        {
            OnBattleEnd?.Invoke();
            StatusManager.ResetStatus();
            StatusManager.BattleEnd();
            AbilityInputProcessor.BattleEnd();
            animator.SetTrigger("StopBattle");
        }

        public bool Select()
        {
            if (dead)
                return false;
            OnSelected?.Invoke(true);
            selected = true;
            return true;
        }

        public void Unselect()
        {
            OnSelected?.Invoke(false);
            selected = false;
        }

        public void Die()
        {
            dead = true;
            if (selected == true)
            {
                battleSystem.Reselect();
            }
        }

        public void SetBattleSystem(BattleSystem battleSystem)
        {
            this.battleSystem = battleSystem;
        }

        public void SetEnemies(CharacterBattleManager[] enemies)
        {
            this.enemies = enemies;
        }

        public void SetAllies(CharacterBattleManager[] allies)
        {
            this.allies = allies;
            if (allies.Length > 1 && allies[0] != this)
            {
                int selfIndex = 0;
                for (int i = 0; i < allies.Length; ++i)
                {
                    if (allies[i] == this)
                    {
                        selfIndex = i;
                        break;
                    }
                }
                var temp = allies[0];
                allies[0] = this;
                allies[selfIndex] = temp;
            }
        }

        public AbilityExecutionData Tick(float delta, float battleSpeedManager)
        {
            abilitiesToUse.Clear();
            if (dead)
                return new AbilityExecutionData() { index = -1, selectedTarget = null };
            StatusManager.ProcessStatus(delta, battleSpeedManager);
            int targetSelectionInput = TargetSelector.GetTargetSelection();
            if (targetSelectionInput == -1) battleSystem.SelectPrevious();
            if (targetSelectionInput == 1) battleSystem.SelectNext();
            AbilityExecutionData abilityExecutionData = AbilityInputProcessor.GetAbilitiesToExecute(StatusManager, StatusManager.GetValueModifier(StatusType.Speed) * delta, delta);
            return abilityExecutionData;
        }

        public IEnumerator ExecuteSkill(int index, CharacterBattleManager cbm)
        {
            BattleStatusManager[] statuses;
            if (cbm == null)
            {
                statuses = GetStatuses(selectedTarget);
            } else
            {
                statuses = GetStatuses(cbm);
            }
            var allyStatuses = GetAllyStatuses();
            yield return AbilityExecutor.ExecuteAbility(index, allyStatuses, statuses);
        }

        protected virtual BattleStatusManager[] GetStatuses(CharacterBattleManager executionSelectedTarget)
        {            
            BattleStatusManager[] statuses = new BattleStatusManager[enemies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = enemies[i].StatusManager;
            }
            if (executionSelectedTarget != null && !IsAlly(executionSelectedTarget))
            {
                if (statuses[0] != executionSelectedTarget.StatusManager)
                {
                    var temp = statuses[0];
                    statuses[0] = executionSelectedTarget.StatusManager;
                    for (int i = 1; i < statuses.Length; ++i)
                    {
                        if (statuses[i] == executionSelectedTarget.StatusManager)
                        {
                            statuses[i] = temp;
                        }
                    }
                }
            }
            return statuses;
        }

        private BattleStatusManager[] GetAllyStatuses()
        {
            BattleStatusManager[] statuses = new BattleStatusManager[allies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = allies[i].StatusManager;
            }
            if (statuses[0] != this.StatusManager)
            {
                var temp = statuses[0];
                statuses[0] = this.StatusManager;
                for (int i = 1; i < statuses.Length; ++i)
                {
                    if (statuses[i] == this.StatusManager)
                    {
                        statuses[i] = temp;
                    }
                }
            }
            return statuses;
        }

        private bool IsAlly(CharacterBattleManager cbm)
        {
            foreach(CharacterBattleManager manager in allies)
            {
                if (manager == cbm)
                    return true;
            }
            return false;
        }

        public bool IsAlly(BattleStatusManager bsm)
        {
            foreach(CharacterBattleManager manager in allies)
            {
                if (manager.StatusManager == bsm)
                    return true;
            }
            return false;
        }

        public void PerformTimeStop(bool activate)
        {
            AbilityInputProcessor.PerformTimeStop(activate);
        }

        public void SetSelectedTarget(CharacterBattleManager cbm)
        {
            selectedTarget = cbm;
            ITargetDependant targetDependantInput = (ITargetDependant)AbilityInputProcessor;
            if (targetDependantInput != null)
            {
                targetDependantInput.SetSelectedTarget(cbm);
            }
        }

        public CharacterBattleManager GetSelectedTarget()
        {
            return selectedTarget;
        }
    }
}