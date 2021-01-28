﻿using Laresistance.Battle;
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
            public UnityEvent<bool> OnAvailabilityChanged = default;
            public UnityEvent<bool> OnAvailabilityChangedColor = default;
            public UnityEvent<BattleAbility> OnShowableAbility = default;
        }

        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;
        [Header("Current abilities")]
        [SerializeField]
        private List<CardSubscription> subscriptions = default;
        [Header("Shuffle and ulti")]
        [SerializeField]
        private UnityEvent<float> OnNextCardProgress = default;
        [SerializeField]
        private UnityEvent<float> OnNextShuffleProgress = default;
        [SerializeField]
        private RectTransform[] cardsLocation = default;
        [SerializeField]
        private RectTransform nextCardIndicator = default;
        [SerializeField]
        private RectTransform ultimatePosition = default;
        [Header("Next Ability queue")]
        [SerializeField]
        private ScriptablePool nextAbilityQueuePool = default;
        [SerializeField]
        private Transform nextAbilityQueueParent = default;
        [Header("Ability to use queue")]
        [SerializeField]
        private ScriptablePool abilityToUsePool = default;
        [SerializeField]
        private Transform abilityToUseQueueParent = default;

        private ScriptablePool particlesPool;

        private void Awake()
        {
            particlesPool = PoolInitializerBehaviour.GetPool("Energy");
            nextAbilityQueuePool.InitPool();
            abilityToUsePool.InitPool();
        }

        private void OnEnable()
        {
            PlayerAbilityInput playerInput = GetPlayerInput();
            playerInput.OnAvailableSkillsChanged += OnAvailableSkillsChanged;
            playerInput.OnNextCardProgress += OnNextCardProgressChanged;
            playerInput.OnNextShuffleProgress += OnNextShuffleProgressChanged;
            playerInput.OnAbilityOnQueue += OnAbilityOnQueue;
            playerInput.OnShuffle += OnShuffle;
            playerInput.OnRenewAbilities += OnRenewedAbilities;
            playerInput.OnAbilityOffQueue += OnAbilityOffQueue;
            playerInput.OnAbilitiesToUseChanged += OnAbilitiesToUseChanged;
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
            playerInput.OnRenewAbilities -= OnRenewedAbilities;
            playerInput.OnAbilityOffQueue -= OnAbilityOffQueue;
            playerInput.OnAbilitiesToUseChanged -= OnAbilitiesToUseChanged;
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
                    subscriptions[i].OnShowableAbility?.Invoke(availableAbilities[i]);
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
            subscriptions[slot].OnAvailabilityChangedColor?.Invoke(!onQueue);    
        }

        private void OnAbilitiesToUseChanged(PlayerAbilityInput sender)
        {
            // Remove instances that are currently active
            abilityToUsePool.SoftFreeAll();

            // Create the new instances with the correct graphics
            foreach (int index in sender.abilitiesToUseIndexList)
            {
                if (index != 4)
                {
                    BattleAbility ability = sender.AvailableAbilities[index];
                    GameObject instance = abilityToUsePool.GetInstance(abilityToUseQueueParent);
                    instance.GetComponent<ShowableAbility>().SetupShowableElement(ability);
                    instance.transform.localScale = Vector3.one;
                }
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

        private void OnRenewedAbilities(PlayerAbilityInput sender)
        {
            nextAbilityQueuePool.SoftFreeAll();
            foreach (BattleAbility ability in sender.nextAbilitiesQueue)
            {
                GameObject instance = nextAbilityQueuePool.GetInstance(nextAbilityQueueParent);
                instance.GetComponent<ShowableAbility>().SetupShowableElement(ability);
                instance.transform.localScale = Vector3.one;
            }
        }

        private void OnAbilityOffQueue(PlayerAbilityInput sender)
        {
            if (nextAbilityQueueParent.childCount > 0)
            {
                nextAbilityQueuePool.FreeInstance(nextAbilityQueueParent.GetChild(0).gameObject);
            }
        }
    }
}