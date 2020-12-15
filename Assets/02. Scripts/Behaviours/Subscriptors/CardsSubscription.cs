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

        private void OnEnable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged += OnAvailableSkillsChanged;
            OnAvailableSkillsChanged(playerInput, playerInput.AvailableAbilities);
        }

        private void OnDisable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged -= OnAvailableSkillsChanged;
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
                } else
                {
                    subscriptions[i].OnAvailabilityChanged?.Invoke(true);
                    subscriptions[i].OnSpriteChanged?.Invoke(availableAbilities[i].AbilityIcon);
                }
            }
        }
    }
}