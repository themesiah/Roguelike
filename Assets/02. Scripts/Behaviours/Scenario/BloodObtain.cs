using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Systems;
using Laresistance.Data;
using UnityEngine;
using System.Collections;
using GamedevsToolbox.ScriptableArchitecture.Events;

namespace Laresistance.Behaviours
{
    public class BloodObtain : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField]
        private ScriptableIntReference baseBloodAmount = default;
        [SerializeField]
        private ScriptableIntReference currentFloor = default;

        [Header("References")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private RewardUILibrary uiLibrary = default;
        [SerializeField]
        private StringGameEvent gameContextSignal = default;
        [SerializeField]
        private SpriteRenderer spriteRenderer = default;

        private RewardSystem rewardSystem;

        private void Start()
        {
            playerDataRef.RegisterOnSetEvent((playerDataBehaviour) => {
                rewardSystem = new RewardSystem(playerDataBehaviour.player, bloodReference, hardCurrencyReference, uiLibrary);
            });
            //rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, uiLibrary);
        }

        public void Obtain()
        {
            int amount = GetBloodAmount();
            StartCoroutine(RewardCoroutine(amount));
        }

        private int GetBloodAmount()
        {
            return baseBloodAmount.GetValue() * currentFloor.GetValue();
        }

        private IEnumerator RewardCoroutine(int amount)
        {
            spriteRenderer.enabled = false;
            gameContextSignal.Raise("UI");
            yield return rewardSystem.GetReward(new RewardData(amount, 0, null, null, null, null));
            gameContextSignal.Raise("Map");
            Destroy(gameObject);
        }
    }
}