using Laresistance.Battle;
using Laresistance.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class AbilitiesSubscription : MonoBehaviour
    {
        [System.Serializable]
        public class AbilitySubscription
        {
            public int abilityIndex = -1;
            public UnityEvent OnAbilityDoesNotExist = default;
            public UnityEvent OnAbilityExists = default;
            public UnityEvent<bool> OnAvailabilityChanged = default;
            public UnityEvent<string> OnAbilityCost = default;
            public UnityEvent<bool> OnAbilityHaveCost = default;
            public UnityEvent<float> OnProgress = default;
        }


        [SerializeField]
        private CharacterBattleBehaviour battleBehaviour = default;
        [SerializeField]
        private List<AbilitySubscription> subscriptions = default;
        [SerializeField]
        private UnityEvent<float> OnRemainingEnergy = default;
        [SerializeField]
        private UnityEvent<int> OnUsableEnergy = default;
        [SerializeField]
        private UnityEvent<string> OnUsableEnergyStr = default;

        private void Start()
        {
        }

        private void OnEnable()
        {
            BattleAbility[] abilities = battleBehaviour.GetAllAbilities();
            foreach (var suscription in subscriptions)
            {
                if (suscription.abilityIndex < 0 || suscription.abilityIndex >= abilities.Length || (abilities[suscription.abilityIndex] == null && suscription.abilityIndex != 19))
                {
                    suscription.OnAbilityDoesNotExist?.Invoke();
                    suscription.OnAbilityHaveCost?.Invoke(false);
                }
                else
                {
                    suscription.OnAbilityExists?.Invoke();
                    int cost = abilities[suscription.abilityIndex].GetCost();
                    if (cost == 0)
                    {
                        suscription.OnAbilityHaveCost?.Invoke(false);
                    }
                    else
                    {
                        suscription.OnAbilityHaveCost?.Invoke(true);
                        suscription.OnAbilityCost?.Invoke(cost.ToString());
                    }
                }
            }

            EnergyChanged(battleBehaviour.StatusManager.CurrentEnergy, battleBehaviour.StatusManager.UsableEnergy);
            battleBehaviour.StatusManager.OnEnergyChanged += EnergyChanged;
            battleBehaviour.StatusManager.OnNextAbilityChanged += OnNextAbilityChanged;
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.OnEnergyChanged -= EnergyChanged;
            battleBehaviour.StatusManager.OnNextAbilityChanged -= OnNextAbilityChanged;
        }

        private void EnergyChanged(float currentEnergy, int usableEnergy)
        {
            OnUsableEnergy?.Invoke(usableEnergy);
            OnRemainingEnergy?.Invoke(currentEnergy - (float)usableEnergy);
            OnUsableEnergyStr?.Invoke(usableEnergy.ToString());
            BattleAbility[] abilities = battleBehaviour.GetAllAbilities();
            foreach (var subscription in subscriptions)
            {
                if (subscription.abilityIndex >= 0 && subscription.abilityIndex < abilities.Length && abilities[subscription.abilityIndex] != null)
                {

                    //subscription.OnAvailabilityChanged?.Invoke(battleBehaviour.StatusManager.CanExecute(abilities[subscription.abilityIndex].GetCost()));
                    subscription.OnAvailabilityChanged?.Invoke(abilities[subscription.abilityIndex].CanBeUsed());
                    subscription.OnProgress?.Invoke(currentEnergy / abilities[subscription.abilityIndex].GetCost());
                }
            }
        }

        private void OnNextAbilityChanged(BattleAbility nextAbility)
        {
            BattleAbility[] abilities = battleBehaviour.GetAllAbilities();
            foreach (var subscription in subscriptions)
            {
                if (subscription.abilityIndex >= 0 && subscription.abilityIndex < abilities.Length && abilities[subscription.abilityIndex] != null)
                {
                    if (nextAbility == abilities[subscription.abilityIndex])
                    {
                        subscription.OnAbilityExists?.Invoke();
                    } else
                    {
                        subscription.OnAbilityDoesNotExist?.Invoke();
                    }
                }
            }
        }
    }
}