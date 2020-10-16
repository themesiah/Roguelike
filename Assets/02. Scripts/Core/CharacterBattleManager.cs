using Laresistance.Battle;
using Laresistance.Behaviours;
using System.Collections;
using UnityEngine;

namespace Laresistance.Core
{
    public class CharacterBattleManager
    {
        public BattleStatusManager StatusManager { get; protected set; }
        public IAbilityInputProcessor AbilityInputProcessor { get; protected set; }
        public IAbilityExecutor AbilityExecutor { get; protected set; }
        protected IBattleAnimator animator;

        private CharacterBattleManager[] allies;
        private CharacterBattleManager[] enemies;

        public delegate void OnBattleStartHandler();
        public event OnBattleStartHandler OnBattleStart;
        public delegate void OnBattleEndHandler();
        public event OnBattleEndHandler OnBattleEnd;

        public CharacterBattleManager(BattleStatusManager statusManager, IAbilityInputProcessor inputProcessor, IAbilityExecutor abilityExecutor, IBattleAnimator animator)
        {
            StatusManager = statusManager;
            AbilityInputProcessor = inputProcessor;
            AbilityExecutor = abilityExecutor;
            this.animator = animator;
        }

        public void StartBattle()
        {
            OnBattleStart.Invoke();
        }

        public void EndBattle()
        {
            OnBattleEnd.Invoke();
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

        public int Tick(float delta)
        {
            StatusManager.ProcessStatus(Time.deltaTime);
            int index = AbilityInputProcessor.GetAbilityToExecute(StatusManager, Time.deltaTime);
            return index;
        }

        public IEnumerator ExecuteSkill(int index)
        {
            var statuses = GetStatuses();
            var allyStatuses = GetAllyStatuses();
            yield return AbilityExecutor.ExecuteAbility(index, allyStatuses, statuses);
        }

        protected virtual BattleStatusManager[] GetStatuses()
        {
            BattleStatusManager[] statuses = new BattleStatusManager[enemies.Length];
            for (int i = 0; i < statuses.Length; ++i)
            {
                statuses[i] = enemies[i].StatusManager;
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