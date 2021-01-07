using Laresistance.Behaviours;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class PlayerAbilityInput : IAbilityInputProcessor
    {
        private Player player;
        private BattleStatusManager battleStatus;

        private int currentAbilityIndex = -1;
        private BattleAbility[] availableAbilities;
        private float renewTimer;
        private float shuffleTimer;

        public BattleAbility[] AvailableAbilities { get { return availableAbilities; } }
        public float NextCardProgress { get { return 1f-(renewTimer / GameConstantsBehaviour.Instance.cardRenewCooldown.GetValue()); } }
        public float NextShuffleProgress { get { return 1f - (shuffleTimer / GameConstantsBehaviour.Instance.shuffleCooldown.GetValue()); } }


        public delegate void OnAvailableSkillsChangedHandler(PlayerAbilityInput sender, BattleAbility[] availableAbilities);
        public event OnAvailableSkillsChangedHandler OnAvailableSkillsChanged;
        public delegate void OnNextCardProgressHandler(PlayerAbilityInput sender, float progress);
        public event OnNextCardProgressHandler OnNextCardProgress;
        public delegate void OnNextShuffleProgressHandler(PlayerAbilityInput sender, float progress);
        public event OnNextShuffleProgressHandler OnNextShuffleProgress;
        public delegate void OnAbilityOnQueueHandler(PlayerAbilityInput sender, int slot, bool onQueue);
        public event OnAbilityOnQueueHandler OnAbilityOnQueue;
        public delegate void OnShuffleHandler(PlayerAbilityInput sender, int[] shuffled);
        public event OnShuffleHandler OnShuffle;

        public PlayerAbilityInput(Player player, BattleStatusManager battleStatus)
        {
            this.player = player;
            this.battleStatus = battleStatus;
            this.battleStatus.OnAbilityExecutionCancelledByTargetDeath += OnAbilityCancelledByTargetDeath;
            this.battleStatus.OnAbilityExecuted += OnAbilityExecuted;

            //renewTimer = GameConstantsBehaviour.Instance.cardRenewCooldown.GetValue();
            //shuffleTimer = GameConstantsBehaviour.Instance.shuffleCooldown.GetValue();
    }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            UpdateAvailableAbilities(delta);
            if (!BattleAbilityManager.Executing)
            {
                shuffleTimer -= delta;
                OnNextShuffleProgress?.Invoke(this, NextShuffleProgress);
            }

            foreach (var ability in player.GetAbilities())
            {
                ability?.Tick(delta);
            }
            int temp = -1;
            if (currentAbilityIndex != -1 && currentAbilityIndex != 4)
            {
                temp = IndexOfAbility(availableAbilities[currentAbilityIndex]);
                OnAbilityOnQueue?.Invoke(this, currentAbilityIndex, true);
                //availableAbilities[currentAbilityIndex] = null;
                OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            } else if (currentAbilityIndex == 4)
            {
                temp = 4;
            }
            currentAbilityIndex = -1;
            return temp;
        }

        public void TryToExecuteAbility(int index)
        {
            if (index < -1 || index > 5)
                throw new System.Exception("Invalid index trying to execute ability. It must be 0,1,2,3 (abilities) or 4 (ultimate)");

            BattleAbility ability;
            if (index == 4)
            {
                ability = player.ultimateAbility;
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

        public void BattleStart()
        {
            InitializeCards();
            shuffleTimer = GameConstantsBehaviour.Instance.shuffleCooldown.GetValue();
            renewTimer = GameConstantsBehaviour.Instance.cardRenewCooldown.GetValue();
        }

        public void BattleEnd()
        {
            for (int i = 0; i < 4; i++)
            {
                OnAbilityOnQueue?.Invoke(this, i, false);
            }
        }

        public BattleAbility[] GetAbilities()
        {
            return player.GetAbilities();
        }

        public void Reshuffle()
        {
            if (!BattleAbilityManager.Executing && shuffleTimer <= 0f && battleStatus.Stunned == false)
            {
                renewTimer = GameConstantsBehaviour.Instance.cardRenewCooldown.GetValue();
                OnNextCardProgress?.Invoke(this, NextCardProgress);
                int[] discarded = DiscardAvailableAbilities();
                OnShuffle?.Invoke(this, discarded);
                battleStatus.AddEnergy(discarded.Length);
                RenewAllAbilities();
                shuffleTimer = GameConstantsBehaviour.Instance.shuffleCooldown.GetValue();
                OnNextShuffleProgress?.Invoke(this, NextShuffleProgress);
            }
        }

        private void InitializeCards()
        {
            var abilities = GetAbilities();
            foreach(var ability in abilities)
            {
                ability?.ResetCooldown();
            }
            UpdateAvailableAbilities(0f);
            RenewAllAbilities();
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
            if (!BattleAbilityManager.Executing)
            {
                renewTimer -= delta;
                OnNextCardProgress?.Invoke(this, NextCardProgress);
            }

            if (delta != 0f)
            {
                RenewAbility();
                OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            }
        }

        private void RenewAbility() // NATURAL CARD RENEWAL
        {
            if (renewTimer > 0f)
                return;
            List<BattleAbility> readyAbilities = new List<BattleAbility>();
            foreach (BattleAbility ability in GetAbilities())
            {
                if (ability != null && ability.CanBeUsed() && !AlreadyInAvailableAbilities(ability))
                {
                    readyAbilities.Add(ability);
                }
            }

            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (readyAbilities.Count == 0)
                    break;
                if (availableAbilities[i] == null)
                {
                    int index = Random.Range(0, readyAbilities.Count);
                    availableAbilities[i] = readyAbilities[index];
                    readyAbilities[index].CurrentPlayerSlot = i;
                    OnAbilityOnQueue?.Invoke(this, i, false);
                    renewTimer = GameConstantsBehaviour.Instance.cardRenewCooldown.GetValue();
                    OnNextCardProgress?.Invoke(this, NextCardProgress);
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
            availableAbilities[slot] = null;
            OnAbilityOnQueue?.Invoke(this, slot, false);
            OnAvailableSkillsChanged?.Invoke(this, AvailableAbilities);
        }

        private void RenewAllAbilities() // SHUFFLE
        {
            List<BattleAbility> readyAbilities = new List<BattleAbility>();
            foreach (BattleAbility ability in GetAbilities())
            {
                if (ability != null && ability.CanBeUsed() && !AlreadyInAvailableAbilities(ability))
                {
                    readyAbilities.Add(ability);
                }
            }

            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (readyAbilities.Count == 0)
                    break;
                int index = Random.Range(0, readyAbilities.Count);
                availableAbilities[i] = readyAbilities[index];
                readyAbilities[index].CurrentPlayerSlot = i;
                OnAbilityOnQueue?.Invoke(this, i, false);
                readyAbilities.RemoveAt(index);
            }
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
        }

        private int[] DiscardAvailableAbilities()
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
                        return i + 1;
                }
            }
            return -1;
        }
    }
}