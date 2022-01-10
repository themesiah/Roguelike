using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;
using System.Collections;
using UnityEngine;

namespace Laresistance.Systems
{
    public class RewardSystemBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
        [SerializeField]
        private RewardUILibrary uiLibrary = default;

        private RewardSystem rewardSystem;

        private void Start()
        {
            rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, uiLibrary);
        }

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        public void Register()
        {
            rewardEvent?.RegisterListener(this);
        }

        public void Unregister()
        {
            rewardEvent?.UnregisterListener(this);
        }

        public virtual IEnumerator OnEventRaised(RewardData data)
        {
            yield return rewardSystem.GetReward(data);
        }
    }
}