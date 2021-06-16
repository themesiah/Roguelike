using Laresistance.Battle;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using GamedevsToolbox.ScriptableArchitecture.Pools;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class CardsSubscription : MonoBehaviour
    {
        [System.Serializable]
        public class CardSubscription {
            public UnityEvent<bool> OnAvailabilityChanged = default;
            public UnityEvent<bool> OnAvailabilityChangedColor = default;
            public UnityEvent<BattleAbility> OnShowableAbility = default;
            public UnityEvent OnUpdateAbility = default;
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
        [Header("Ability used animation")]
        [SerializeField]
        private Transform animationPivot = default;
        [SerializeField]
        private GameObject animationIconPrefab = default;


        private ScriptablePool particlesPool;
        private RectTransform emptyQueueAnimatorObject;
        private List<ShowableAbility> nextShowableAbilities;

        private void Awake()
        {
            nextShowableAbilities = new List<ShowableAbility>();
            particlesPool = PoolInitializerBehaviour.GetPool("Energy");
            nextAbilityQueuePool.InitPool();
            abilityToUsePool.InitPool();
            GameObject go = new GameObject("EmptyQueueAnimator");
            emptyQueueAnimatorObject = go.AddComponent<RectTransform>();
            emptyQueueAnimatorObject.gameObject.SetActive(false);
            emptyQueueAnimatorObject.SetParent(null);
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
            playerInput.OnAbilityExecutedFromQueue += OnAbilityExecutedFromQueue;
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
            playerInput.OnAbilityExecutedFromQueue -= OnAbilityExecutedFromQueue;
        }

        private PlayerAbilityInput GetPlayerInput()
        {
            return (PlayerAbilityInput)battleBehaviour.AbilityInputProcessor;
        }

        public void OnAbilitiesNeedUpdate()
        {
            foreach(var showableAbilitySubscription in subscriptions)
            {
                showableAbilitySubscription?.OnUpdateAbility?.Invoke();
            }
            foreach (var showableAbility in nextShowableAbilities)
            {
                showableAbility.UpdateAbilityValues();
            }
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
                    foreach(var showableAbility in nextShowableAbilities)
                    {
                        showableAbility.UpdateAbilityValues();
                    }
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
            if (slot >= 0 && slot <= 3)
            {
                subscriptions[slot].OnAvailabilityChangedColor?.Invoke(!onQueue);
            }
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
                } else
                {
                    BattleAbility ability = sender.PlayerUltimate;
                    GameObject instance = abilityToUsePool.GetInstance(abilityToUseQueueParent);
                    instance.GetComponent<ShowableAbility>().SetupShowableElement(ability);
                    instance.transform.localScale = Vector3.one;
                }
            }
        }

        private void OnShuffle(PlayerAbilityInput sender, int[] discarded)
        {
            particlesPool.SoftFreeAll();
            foreach(int discardedIndex in discarded)
            {
                GameObject particleObject = particlesPool.GetInstance(null, cardsLocation[discardedIndex].position, Quaternion.identity);
                UltimateParticlesEffect upe = particleObject.GetComponent<UltimateParticlesEffect>();
                upe.GoToTarget(ultimatePosition.position, particlesPool);
            }
        }

        private void OnRenewedAbilities(PlayerAbilityInput sender)
        {
            DeactivateQueueAnimator();
            nextAbilityQueuePool.SoftFreeAll();
            foreach (BattleAbility ability in sender.nextAbilitiesQueue)
            {
                GameObject instance = nextAbilityQueuePool.GetInstance(nextAbilityQueueParent);
                ShowableAbility sa = instance.GetComponent<ShowableAbility>();
                sa.SetupShowableElement(ability);
                nextShowableAbilities.Add(sa);
                instance.transform.localScale = Vector3.one;
            }
        }

        private void OnAbilityOffQueue(PlayerAbilityInput sender)
        {
            if (nextAbilityQueueParent.childCount > 0)
            {
                nextAbilityQueuePool.FreeInstance(nextAbilityQueueParent.GetChild(0).gameObject);
                nextShowableAbilities.RemoveAt(0);
            }
            ActivateQueueAnimator();
        }

        private void OnAbilityExecutedFromQueue(PlayerAbilityInput sender, BattleAbility ability)
        {
            GameObject animationIconInstance = Instantiate(animationIconPrefab, animationPivot, false);
            animationIconInstance.GetComponent<ShowableAbility>().SetupShowableElement(ability);
            animationIconInstance.transform.localPosition = Vector3.zero;
        }

        private void ActivateQueueAnimator()
        {
            emptyQueueAnimatorObject.SetParent(nextAbilityQueueParent);
            emptyQueueAnimatorObject.SetAsFirstSibling();
            emptyQueueAnimatorObject.localScale = Vector3.one;
            emptyQueueAnimatorObject.sizeDelta = new Vector2(1f, 100f);
            emptyQueueAnimatorObject.gameObject.SetActive(true);
            StartCoroutine(QueueAnimatorActivationCoroutine());
        }

        private void DeactivateQueueAnimator()
        {
            emptyQueueAnimatorObject.gameObject.SetActive(true);
            emptyQueueAnimatorObject.SetParent(null);
        }

        private IEnumerator QueueAnimatorActivationCoroutine()
        {
            float timer = 1f;

            while (timer > 0f)
            {
                timer -= Time.deltaTime;

                emptyQueueAnimatorObject.sizeDelta = Vector2.right + Vector2.up * timer * 100f;
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)nextAbilityQueueParent);
                Canvas.ForceUpdateCanvases();

                yield return null;
            }

            DeactivateQueueAnimator();
        }
    }
}