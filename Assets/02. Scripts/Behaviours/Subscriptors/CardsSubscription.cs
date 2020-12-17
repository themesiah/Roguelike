using Laresistance.Battle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class CardsSubscription : MonoBehaviour
    {
        [System.Serializable]
        public class CardSubscription {
            public UnityEvent<Sprite> OnSpriteChanged = default;
            public UnityEvent<bool> OnAvailabilityChanged = default;
        }

        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;
        [SerializeField]
        private List<CardSubscription> subscriptions = default;
        [SerializeField]
        private UnityEvent<float> OnNextCardProgress = default;
        [SerializeField]
        private UnityEvent<float> OnNextShuffleProgress = default;

        private void OnEnable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged += OnAvailableSkillsChanged;
            playerInput.OnNextCardProgress += OnNextCardProgressChanged;
            playerInput.OnNextShuffleProgress += OnNextShuffleProgressChanged;
            OnAvailableSkillsChanged(playerInput, playerInput.AvailableAbilities);
            OnNextCardProgressChanged(playerInput, playerInput.NextCardProgress);
            OnNextShuffleProgressChanged(playerInput, playerInput.NextShuffleProgress);
        }

        private void OnDisable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged -= OnAvailableSkillsChanged;
            playerInput.OnNextCardProgress -= OnNextCardProgressChanged;
            playerInput.OnNextShuffleProgress -= OnNextShuffleProgressChanged;
        }

        private PlayerAbilityInput GetPlayerInput()
        {
            return (PlayerAbilityInput)battleBehaviour.AbilityInputProcessor;
        }

        private void OnAvailableSkillsChanged(PlayerAbilityInput sender, BattleAbility[] availableAbilities)
        {
            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (availableAbilities[i] == null)
                {
                    subscriptions[i].OnAvailabilityChanged?.Invoke(false);
                } else if (subscriptions.Count > i)
                {
                    subscriptions[i].OnAvailabilityChanged?.Invoke(true);
                    subscriptions[i].OnSpriteChanged?.Invoke(availableAbilities[i].AbilityIcon);
                }
            }
        }

        private void OnNextCardProgressChanged(PlayerAbilityInput sender, float progress)
        {
            OnNextCardProgress?.Invoke(progress);
        }

        private void OnNextShuffleProgressChanged(PlayerAbilityInput sender, float progress)
        {
            OnNextShuffleProgress?.Invoke(progress);
        }
    }
}