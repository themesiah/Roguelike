using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Laresistance.Battle
{
    public class PlayerAbilityInput : IAbilityInputProcessor
    {
        private static float RENEW_CARD_COOLDOWN = 3f;

        private Player player;
        private BattleStatusManager battleStatus;

        private int currentAbilityIndex = -1;
        private BattleAbility[] availableAbilities;
        private float renewTimer = RENEW_CARD_COOLDOWN;

        public BattleAbility[] AvailableAbilities { get { return availableAbilities; } }


        public delegate void OnAvailableSkillsChangedHandler(PlayerAbilityInput sender, BattleAbility[] availableAbilities);
        public event OnAvailableSkillsChangedHandler OnAvailableSkillsChanged;

        public PlayerAbilityInput(Player player, BattleStatusManager battleStatus)
        {
            this.player = player;
            this.battleStatus = battleStatus;
        }

        public int GetAbilityToExecute(BattleStatusManager battleStatus, float delta)
        {
            UpdateAvailableAbilities(delta);

            foreach(var ability in player.GetAbilities())
            {
                ability?.Tick(delta);
            }
            int temp = -1;
            if (currentAbilityIndex != -1)
            {
                temp = IndexOfAbility(availableAbilities[currentAbilityIndex]);
                availableAbilities[currentAbilityIndex] = null;
                OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            }
            currentAbilityIndex = -1;
            return temp;
        }

        public void TryToExecuteAbility(int index)
        {
            if (index < -1 || index > 4)
                throw new System.Exception("Invalid index trying to execute ability. It must be 0 (character ability), 1,2,3 (minion abilities) or 4,5,6 (consumables)");
            var ability = availableAbilities[index];
            if (ability != null && ability.CanBeUsed())
            {
                currentAbilityIndex = index;
            }
        }

        public void BattleStart()
        {
            InitializeCards();
        }

        public BattleAbility[] GetAbilities()
        {
            return player.GetAbilities();
        }

        public void Reshuffle()
        {
            renewTimer = RENEW_CARD_COOLDOWN;
            int discarded = DiscardAvailableAbilities();
            battleStatus.AddEnergy(discarded);
            RenewAllAbilities();
        }

        private void InitializeCards()
        {
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
            renewTimer -= delta;

            RenewAbility();
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
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
                    renewTimer = RENEW_CARD_COOLDOWN;
                    break;
                }
            }
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
                readyAbilities.RemoveAt(index);
            }
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
        }

        private int DiscardAvailableAbilities()
        {
            int amountDiscarded = 0;
            foreach (var ability in availableAbilities)
            {
                if (ability != null)
                {
                    amountDiscarded++;
                    ability.SetCooldownAsUsed();
                }
            }
            OnAvailableSkillsChanged?.Invoke(this, availableAbilities);
            return amountDiscarded;
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
                    return i;
                }
            }
            return -1;
        }
    }
}