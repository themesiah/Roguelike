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
            BattleAbility[] abilities = battleBehaviour.GetAbilities();
            foreach (var suscription in subscriptions)
            {
                if (suscription.abilityIndex < 0 || suscription.abilityIndex >= abilities.Length || abilities[suscription.abilityIndex] == null)
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
        }

        private void OnDisable()
        {
            battleBehaviour.StatusManager.OnEnergyChanged -= EnergyChanged;
        }

        private void EnergyChanged(float currentEnergy, int usableEnergy)
        {
            OnUsableEnergy?.Invoke(usableEnergy);
            OnRemainingEnergy?.Invoke(currentEnergy - (float)usableEnergy);
            OnUsableEnergyStr?.Invoke(usableEnergy.ToString());
            BattleAbility[] abilities = battleBehaviour.GetAbilities();
            foreach (var suscription in subscriptions)
            {
                if (suscription.abilityIndex >= 0 && suscription.abilityIndex < abilities.Length && abilities[suscription.abilityIndex] != null)
                {
                    suscription.OnAvailabilityChanged?.Invoke(battleBehaviour.StatusManager.CanExecute(abilities[suscription.abilityIndex].GetCost()));
                    suscription.OnProgress?.Invoke(currentEnergy / abilities[suscription.abilityIndex].GetCost());
                }
            }
        }
    }
}