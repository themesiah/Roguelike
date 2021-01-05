using Laresistance.Battle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GamedevsToolbox.ScriptableArchitecture.Pools;

namespace Laresistance.Behaviours
{
    public class CardsSubscription : MonoBehaviour
    {
        [System.Serializable]
        public class CardSubscription {
            public UnityEvent<Sprite> OnSpriteChanged = default;
            public UnityEvent<Sprite> OnFrameChanged = default;
            public UnityEvent<Color> OnAvailabilityChangedColor = default;
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
        [SerializeField]
        private Color onQueueColor = default;
        [SerializeField]
        private RectTransform[] cardsLocation = default;
        [SerializeField]
        private RectTransform nextCardIndicator = default;
        [SerializeField]
        private ScriptablePool particlesPool = default;
        [SerializeField]
        private RectTransform ultimatePosition = default;

        private void Awake()
        {
            particlesPool.InitPool();
        }

        private void OnEnable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged += OnAvailableSkillsChanged;
            playerInput.OnNextCardProgress += OnNextCardProgressChanged;
            playerInput.OnNextShuffleProgress += OnNextShuffleProgressChanged;
            playerInput.OnAbilityOnQueue += OnAbilityOnQueue;
            playerInput.OnShuffle += OnShuffle;
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
            playerInput.OnAbilityOnQueue -= OnAbilityOnQueue;
            playerInput.OnShuffle -= OnShuffle;
        }

        private PlayerAbilityInput GetPlayerInput()
        {
            return (PlayerAbilityInput)battleBehaviour.AbilityInputProcessor;
        }

        private void OnAvailableSkillsChanged(PlayerAbilityInput sender, BattleAbility[] availableAbilities)
        {
            bool cardIndicationPositionSelected = false;
            for (int i = 0; i < availableAbilities.Length; ++i)
            {
                if (availableAbilities[i] == null)
                {
                    subscriptions[i].OnAvailabilityChanged?.Invoke(false);
                    if (cardIndicationPositionSelected == false)
                    {
                        nextCardIndicator.position = cardsLocation[i].position;
                        nextCardIndicator.gameObject.SetActive(true);
                        cardIndicationPositionSelected = true;
                    }
                } else if (subscriptions.Count > i)
                {
                    subscriptions[i].OnAvailabilityChanged?.Invoke(true);
                    subscriptions[i].OnSpriteChanged?.Invoke(availableAbilities[i].AbilityIcon);
                    subscriptions[i].OnFrameChanged?.Invoke(availableAbilities[i].AbilityFrame);
                }
            }
            if (cardIndicationPositionSelected == false)
            {
                nextCardIndicator.gameObject.SetActive(false);
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

        private void OnAbilityOnQueue(PlayerAbilityInput sender, int slot, bool onQueue)
        {
            if (onQueue)
            {
                subscriptions[slot].OnAvailabilityChangedColor?.Invoke(onQueueColor);
            }
            else
            {
                subscriptions[slot].OnAvailabilityChangedColor?.Invoke(Color.white);
            }
        }

        private void OnShuffle(PlayerAbilityInput sender, int[] discarded)
        {
            foreach(int discardedIndex in discarded)
            {
                GameObject particleObject = particlesPool.GetInstance(null, cardsLocation[discardedIndex].position, Quaternion.identity);
                UltimateParticlesEffect upe = particleObject.GetComponent<UltimateParticlesEffect>();
                upe.GoToTarget(ultimatePosition.position, particlesPool);
            }
        }
    }
}