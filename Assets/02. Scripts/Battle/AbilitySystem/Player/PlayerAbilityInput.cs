using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laresistance.Battle
{
    public abstract class PlayerAbilityInput : IAbilityInputProcessor, ITargetDependant
    {
        protected Player player;
        protected BattleStatusManager battleStatus;

        protected int currentAbilityIndex = -1;
        private BattleAbility[] availableAbilities;
        protected float renewTimer;
        private float abilitiesToUseDequeueTimer;

        public float NextSupportAbilityProgress { get { return 1f - player.supportAbility.TimerProgress; } }

        public Queue<BattleAbility> nextAbilitiesQueue { get; private set; }

        public BattleAbility[] AvailableAbilities { get { return availableAbilities; } }
        public float NextCardProgress { get { return 1f - (renewTimer / TotalCardRenewCooldown); } }

        protected abstract float SpecificCardRenewCooldown();
        
        public float TotalCardRenewCooldown { get
            {
                float total = SpecificCardRenewCooldown();
                total = player.GetEquipmentContainer().ModifyValue(Equipments.EquipmentSituation.RenewCardDelay, total);
                return total;
            } }
        public List<int> abilitiesToUseIndexList { get; private set; }

        protected List<AbilityExecutionData> abilitiesToUseList;
        private bool timeStopped = false;
        private CharacterBattleManager selectedTarget = null;


        public delegate void OnAvailableSkillsChangedHandler(PlayerAbilityInput sender, BattleAbility[] availableAbilities);
        public event OnAvailableSkillsChangedHandler OnAvailableSkillsChanged;
        public delegate void OnNextCardProgressHandler(PlayerAbilityInput sender, float progress);
        public event OnNextCardProgressHandler OnNextCardProgress;
        public delegate void OnNextShuffleProgressHandler(PlayerAbilityInput sender, float progress);
        public event OnNextShuffleProgressHandler OnNextSupportAbilityProgress;
        public delegate void OnAbilityOnQueueHandler(PlayerAbilityInput sender, int slot, bool onQueue);
        public event OnAbilityOnQueueHandler OnAbilityOnQueue;
        public delegate void OnRenewAbilitiesHandler(PlayerAbilityInput sender);
        public event OnRenewAbilitiesHandler OnRenewAbilities;
        public delegate void OnAbilityOffQueueHandler(PlayerAbilityInput sender);
        public event OnAbilityOffQueueHandler OnAbilityOffQueue;
        public delegate void OnAbilityOnCooldownHandler(PlayerAbilityInput sender, BattleAbility ability);
        public event OnAbilityOnCooldownHandler OnAbilityOnCooldown;
        public delegate void OnAbilitiesToUseChangedHandler(PlayerAbilityInput sender);
        public event OnAbilitiesToUseChangedHandler OnAbilitiesToUseChanged;
        public delegate void OnAbilityExecutedFromQueueHandler(PlayerAbilityInput sender, BattleAbility ability);
        public event OnAbilityExecutedFromQueueHandler OnAbilityExecutedFromQueue;

        public PlayerAbilityInput(Player player, BattleStatusManager battleStatus)
        {
            this.player = player;
            this.battleStatus = battleStatus;
            this.battleStatus.OnAbilityExecutionCancelledByTargetDeath += OnAbilityCancelledByTargetDeath;
            this.battleStatus.OnAbilityExecuted += OnAbilityExecuted;
            nextAbilitiesQueue = new Queue<BattleAbility>();
            abilitiesToUseList = new List<AbilityExecutionData>();
            abilitiesToUseIndexList = new List<int>();
            battleStatus.OnStatusApplied += OnApplyStatus;
            battleStatus.health.OnShieldTotallyDestroyed += OnShieldTotallyDestroyed;
        }

        private void OnApplyStatus(BattleStatusManager sender, StatusIconType statusType, float duration)
        {
            if (statusType == StatusIconType.Stun)
            {
                ClearAbilityToUseQueue();
            }
        }

        private void OnShieldTotallyDestroyed(CharacterHealth health)
        {
            float supportRenew = 0f;
            supportRenew = battleStatus.GetEquipmentsContainer().ModifyValue(Equipments.EquipmentSituation.ShieldDestroyedSupportCooldown, supportRenew);
            if (supportRenew > 0f)
            {
                player.supportAbility.ForceTick(supportRenew, false);
                ExecuteOnNextSupportAbilityProgress(NextSupportAbilityProgress);
            }
        }

        protected abstract void OnBattleStart();
        protected abstract void OnBattleEnd();
        protected abstract bool CanExecuteAbilities();

        protected void ExecuteOnShuffle(int[] shuffled)
        {
            //OnShuffle?.Invoke(this, shuffled);
        }

        protected void ExecuteOnNextSupportAbilityProgress(float progress)
        {
            OnNextSupportAbilityProgress?.Invoke(this, progress);
        }

        protected void ExecuteOnNextCardProgress()
        {
            OnNextCardProgress?.Invoke(this, NextCardProgress);
        }

        protected virtual void ModifyInputDelta(ref float delta)
        {
        }

        protected virtual void Tick(float delta)
        { }

        public AbilityExecutionData GetAbilitiesToExecute(BattleStatusManager battleStatus, float delta, float unmodifiedDelta)
        {
            Tick(delta);
            ModifyInputDelta(ref delta);

            // Update if any current ability has to change or be added
            UpdateAvailableAbilities(delta);

            if (!BattleAbilityManager.Instance.Executing && !BattleAbilityManager.Instance.AbilityInQueue && !battleStatus.Stunned && BattleAbilityManager.Instance.QueueIsEmpty)
            {} else if (!BattleAbilityManager.Instance.Executing && !battleStatus.Stunned)
            {
                abilitiesToUseDequeueTimer -= delta;
            } else if (!BattleAbilityManager.Instance.Executing && battleStatus.Stunned && (abilitiesToUseList.Count > 0 && abilitiesToUseList[0].ability.CanBeUsed()))
            {
                abilitiesToUseDequeueTimer = 0f;
            }

            // Tick abilities to reduce the current cooldown and those things
            if (BattleAbilityManager.Instance.QueueIsEmpty)
            {
                foreach (var ability in player.GetAbilities())
                {
                    ability?.Tick(delta);
                }
                OnAbilitiesUpdate(delta, unmodifiedDelta);
            }

            // Check if the player selected any ability
            if (currentAbilityIndex != -1 && currentAbilityIndex != 4 && !abilitiesToUseIndexList.Contains(currentAbilityIndex))
            {
                AddAbilityToUseQueue(currentAbilityIndex);
            } else if (currentAbilityIndex == 4 && !abilitiesToUseIndexList.Contains(4))
            {
                AddAbilityToUseQueue(currentAbilityIndex);
            }

            // If player selected an ability, deselect it
            currentAbilityIndex = -1;

            if (!timeStopped)
            {
                // Return list of selected abilities
                AbilityExecutionData aed = new AbilityExecutionData() { index = -1, selectedTarget = null, ability = null };
                if (abilitiesToUseList.Count > 0 && abilitiesToUseDequeueTimer <= 0f)
                {
                    aed = DequeueAbilityToUse();
                    OnAbilityExecutedFromQueue?.Invoke(this, aed.ability);
                }

                return aed;
            } else
            {
                // Return an empty list, because time has stopped
                return new AbilityExecutionData() { index = -1, selectedTarget = null, ability = null};
            }
        }

        protected virtual void OnAbilitiesUpdate(float delta, float unmodifiedDelta)
        {

        }

        public void TryToExecuteAbility(int index)
        {
            if (!CanExecuteAbilities())
                return;
            if (index < -1 || index > 5)
                throw new System.Exception("Invalid index trying to execute ability. It must be 0,1,2,3 (abilities) or 4 (ultimate)");

            BattleAbility ability;
            if (index == 4)
            {
                ability = player.ultimateAbility;
            }
            else if (index == 5)
            {
                ability = player.supportAbility;
            }
            else
            {
                ability = availableAbilities[index];
            }
            if (ability != null && ability.CanBeUsed())
            {
                currentAbilityIndex = index;
            }
        }

        private void AddAbilityToUseQueue(int abilityInternalIndex)
        {
            if (abilityInternalIndex == 4)
            {
                abilitiesToUseList.Add(new AbilityExecutionData() { index = abilityInternalIndex, selectedTarget = GetSelectedTarget(), ability = player.ultimateAbility });
                abilitiesToUseIndexList.Add(abilityInternalIndex);
                abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
            } else if (abilityInternalIndex == 5)
            {
                abilitiesToUseList.Add(new AbilityExecutionData() { index = abilityInternalIndex, selectedTarget = null, ability = player.supportAbility });
                abilitiesToUseIndexList.Add(abilityInternalIndex);
                abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
            }
            else
            {
                AbilityExecutionData aed = new AbilityExecutionData() { index = IndexOfAbility(availableAbilities[abilityInternalIndex]), selectedTarget = GetSelectedTarget(), ability = availableAbilities[abilityInternalIndex] };
                if (availableAbilities[abilityInternalIndex].IsPrioritary() && (abilitiesToUseList.Count == 0 || BattleAbilityManager.Instance.Executing))
                {
                    abilitiesToUseList.Insert(0, aed);
                    abilitiesToUseIndexList.Insert(0, abilityInternalIndex);
                    abilitiesToUseDequeueTimer = 0f;
                }
                else
                {
                    abilitiesToUseList.Add(aed);
                    abilitiesToUseIndexList.Add(abilityInternalIndex);
                    abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
                }
            }
            OnAbilitiesToUseChanged?.Invoke(this);
            OnAbilityOnQueue?.Invoke(this, abilityInternalIndex, true);
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            BattleAbilityManager.Instance.AbilityInQueue = true;
        }

        private AbilityExecutionData DequeueAbilityToUse()
        {
            BattleAbility[] abilities = new BattleAbility[abilitiesToUseIndexList.Count];
            for(int i = 0; i < abilitiesToUseIndexList.Count; ++i)
            {
                if (abilitiesToUseIndexList[i] == 4)
                {
                    abilities[i] = player.ultimateAbility;
                } else if (abilitiesToUseIndexList[i] == 5)
                {
                    abilities[i] = player.supportAbility;
                } else
                {
                    abilities[i] = availableAbilities[abilitiesToUseIndexList[i]];
                }
            }
            for (int i = 0; i < player.combos.Length; ++i)
            {
                Combo combo = player.combos[i];
                if (combo.IsSatisfiedBy(abilities))
                {
                    for(int j = 0; j < combo.ComboLength; ++j)
                    {
                        OnAbilityExecuted(availableAbilities[abilitiesToUseIndexList[0]], abilitiesToUseIndexList[0]);
                        abilitiesToUseIndexList.RemoveAt(0);
                        abilitiesToUseList.RemoveAt(0);
                    }
                    abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
                    OnAbilitiesToUseChanged?.Invoke(this);
                    if (abilitiesToUseIndexList.Count == 0)
                    {
                        BattleAbilityManager.Instance.AbilityInQueue = false;
                    }
                    return new AbilityExecutionData() { index = i + 21, selectedTarget = GetSelectedTarget(), ability = combo.comboAbility };
                }
            }

            abilitiesToUseIndexList.RemoveAt(0);
            AbilityExecutionData aed = abilitiesToUseList[0];
            abilitiesToUseList.RemoveAt(0);
            if (abilitiesToUseIndexList.Count != 0 && (abilitiesToUseIndexList[0] == 4 || abilitiesToUseIndexList[0] == 5))
            {
                abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
            } else
            if ((abilitiesToUseIndexList.Count != 0 && availableAbilities[abilitiesToUseIndexList[0]].IsPrioritary()))
            {
                abilitiesToUseDequeueTimer = 0f;
            } else
            {
                abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
            }
            OnAbilitiesToUseChanged?.Invoke(this);
            if (abilitiesToUseIndexList.Count == 0)
            {
                BattleAbilityManager.Instance.AbilityInQueue = false;
            }
            return aed;
        }

        protected void ClearAbilityToUseQueue()
        {
            abilitiesToUseList.Clear();
            abilitiesToUseIndexList.Clear();
            OnAbilitiesToUseChanged?.Invoke(this);
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            BattleAbilityManager.Instance.AbilityInQueue = false;
            for (int i = 0; i < 4; i++)
            {
                OnAbilityOnQueue?.Invoke(this, i, false);
            }
        }

        public void BattleStart()
        {
            abilitiesToUseList.Clear();
            abilitiesToUseIndexList.Clear();
            OnAbilitiesToUseChanged?.Invoke(this);
            InitializeAbilities();
            renewTimer = TotalCardRenewCooldown;
            abilitiesToUseDequeueTimer = GameConstantsBehaviour.Instance.abilityToUseDequeueTimer.GetValue();
            OnBattleStart();
        }

        public void BattleEnd()
        {
            for (int i = 0; i < 4; i++)
            {
                OnAbilityOnQueue?.Invoke(this, i, false);
            }
            OnBattleEnd();
        }

        public BattleAbility[] GetAbilities()
        {
            return player.GetAbilities();
        }

        public int GetAbilitiesCount()
        {
            int count = 0;
            var abilities = GetAbilities();
            foreach (var ability in abilities)
            {
                if (ability != null)
                    count++;
            }
            return count;
        }

        public abstract void SupportAbility(InputAction.CallbackContext context);
        public abstract void UltimateAbility(InputAction.CallbackContext context);

        private void InitializeAbilities()
        {
            var abilities = GetAbilities();
            foreach(var ability in abilities)
            {
                ability?.ResetCooldown();
            }
            UpdateAvailableAbilities(0f);
            RenewAllAbilities();
            OnInitializeAbilities();
        }

        protected virtual void OnInitializeAbilities()
        {

        }

        private void UpdateAvailableAbilities(float delta)
        {
            if (availableAbilities == null)
                availableAbilities = new BattleAbility[4];

            bool needToUpdate = false;
            foreach (var ability in availableAbilities)
            {
                if (ability == null)
                {
                    needToUpdate = true;
                    break;
                }
            }

            if (!needToUpdate)
                return;

            // 1- Set which abilities have complete cooldown and are not in availableAbilities
            // 2- Get a random one for every null in availableAbilities. Remove it.
            if (!BattleAbilityManager.Instance.Executing && !BattleAbilityManager.Instance.AbilityInQueue && !battleStatus.Stunned)
            {
                if (nextAbilitiesQueue.Count != 0)
                {
                    renewTimer -= delta;
                }
                OnNextCardProgress?.Invoke(this, NextCardProgress);
            }

            if (delta != 0f)
            {
                RenewAbility();
                OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            }
        }

        protected void RenewAbility() // NATURAL CARD RENEWAL
        {
            if (renewTimer > 0f)
                return;

            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (nextAbilitiesQueue.Count == 0)
                    break;
                if (availableAbilities[i] == null)
                {
                    availableAbilities[i] = nextAbilitiesQueue.Dequeue();
                    availableAbilities[i].CurrentPlayerSlot = i;
                    OnAbilityOnQueue?.Invoke(this, i, false);
                    renewTimer = TotalCardRenewCooldown;
                    OnNextCardProgress?.Invoke(this, NextCardProgress);
                    OnAbilityOffQueue?.Invoke(this);
                    break;
                }
            }
        }

        private void OnAbilityCancelledByTargetDeath(BattleAbility ability, int slot)
        {
            availableAbilities[slot] = ability;
            OnAbilityOnQueue?.Invoke(this, slot, false);
            OnAvailableSkillsChanged?.Invoke(this, AvailableAbilities);
        }

        private void OnAbilityExecuted(BattleAbility ability, int slot)
        {
            if (availableAbilities.Length > slot && slot >= 0 && !ability.data.IsComboSkill)
            {
                //if (GetAbilitiesCount() == 4 || GetAbilitiesCount() == 5) Commented, so abilities are always rotating, even when you already have minions
                //{
                nextAbilitiesQueue.Enqueue(availableAbilities[slot]);
                OnAbilityOnCooldown?.Invoke(this, availableAbilities[slot]);
                //}
                availableAbilities[slot] = null;
                OnAbilityOnQueue?.Invoke(this, slot, false);
                OnAvailableSkillsChanged?.Invoke(this, AvailableAbilities);
                OnAbilityExecutedExtra(ability, slot);
            }
        }

        protected virtual void OnAbilityExecutedExtra(BattleAbility ability, int slot)
        {

        }

        protected void RenewAllAbilities() // SHUFFLE
        {
            List<BattleAbility> readyAbilities = new List<BattleAbility>();
            foreach (BattleAbility ability in GetAbilities())
            {
                if (ability != null && ability.CanBeUsed())// && !AlreadyInAvailableAbilities(ability))
                {
                    readyAbilities.Add(ability);
                }
            }

            nextAbilitiesQueue.Clear();
            while(readyAbilities.Count > 0)
            {
                int index = Random.Range(0, readyAbilities.Count);
                nextAbilitiesQueue.Enqueue(readyAbilities[index]);
                readyAbilities.RemoveAt(index);
            }

            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (nextAbilitiesQueue.Count == 0)
                    break;
                availableAbilities[i] = nextAbilitiesQueue.Dequeue();
                availableAbilities[i].CurrentPlayerSlot = i;
                OnAbilityOnQueue?.Invoke(this, i, false);
            }
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            OnRenewAbilities?.Invoke(this);
        }

        protected void RenewAllEmptyAbilities() // RUSH
        {
            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (availableAbilities[i] == null)
                {
                    availableAbilities[i] = nextAbilitiesQueue.Dequeue();
                    availableAbilities[i].CurrentPlayerSlot = i;
                    OnAbilityOnQueue?.Invoke(this, i, false);
                }
            }
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            OnRenewAbilities?.Invoke(this);
        }

        protected int[] DiscardAvailableAbilities()
        {
            int amountDiscarded = 0;
            List<int> discarded = new List<int>();
            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                var ability = availableAbilities[i];
                if (ability != null)
                {
                    amountDiscarded++;
                    ability.SetCooldownAsUsed();
                    discarded.Add(i);
                }
            }
            availableAbilities = new BattleAbility[4];
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            return discarded.ToArray();
        }

        private bool AlreadyInAvailableAbilities(BattleAbility ability)
        {
            foreach(var availableAbility in availableAbilities)
            {
                if (ability == availableAbility)
                {
                    return true;
                }
            }
            return false;
        }

        private int IndexOfAbility(BattleAbility ability)
        {
            BattleAbility[] abilities = GetAbilities();
            for (int i = 0; i < abilities.Length; ++i)
            {
                if (abilities[i] == ability)
                {
                    if (i < 4)
                        return i;
                    else
                        return i + 2;
                }
            }
            return -1;
        }

        public void PerformTimeStop(bool activate)
        {
            timeStopped = activate;
        }

        public void SetSelectedTarget(CharacterBattleManager cbm)
        {
            selectedTarget = cbm;
        }

        public CharacterBattleManager GetSelectedTarget()
        {
            return selectedTarget;
        }

        public BattleAbility PlayerUltimate => player.ultimateAbility;
        public BattleAbility PlayerSupport => player.supportAbility;
    }
}