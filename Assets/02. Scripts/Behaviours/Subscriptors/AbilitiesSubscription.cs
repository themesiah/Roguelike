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
            public UnityEvent<float> OnAbilityTimerChanged = default;
        }


        [SerializeField]
        private PlayerDataBehaviour dataBehaviour = default;
        [SerializeField]
        private List<AbilitySubscription> suscriptions = default;

        private void Start()
        {
        }

        private void OnEnable()
        {
            BattleAbility[] abilities = dataBehaviour.player.GetAbilities();
            foreach (var suscription in suscriptions)
            {
                if (suscription.abilityIndex < 0 || suscription.abilityIndex >= abilities.Length || abilities[suscription.abilityIndex] == null)
                {
                    suscription.OnAbilityDoesNotExist?.Invoke();
                }
                else
                {
                    suscription.OnAbilityExists?.Invoke();
                    abilities[suscription.abilityIndex].OnAbilityTimerChanged += (current, cooldown, percent)=> { AbilityTimerChanged(suscription.abilityIndex, current, cooldown, percent); };
                }
            }
        }

        private void OnDisable()
        {
            BattleAbility[] abilities = dataBehaviour.player.GetAbilities();
            foreach (var suscription in suscriptions)
            {
                if (suscription.abilityIndex < 0 || suscription.abilityIndex >= abilities.Length || abilities[suscription.abilityIndex] == null)
                {

                }
                else
                {
                    abilities[suscription.abilityIndex].OnAbilityTimerChanged -= (current, cooldown, percent) => { AbilityTimerChanged(suscription.abilityIndex, current, cooldown, percent); };
                }
            }
        }

        private void AbilityTimerChanged(int index, float currentTimer, float cooldown, float percent)
        {
            suscriptions[index].OnAbilityTimerChanged?.Invoke(percent);
        }
    }
}