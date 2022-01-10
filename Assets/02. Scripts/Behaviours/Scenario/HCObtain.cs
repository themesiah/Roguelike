using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Data;
using Laresistance.Systems;
using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class HCObtain : MonoBehaviour
    {
        [SerializeField]
        private int amountObtain = 1;
        [Header("References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private IntGameEvent hcObtainedEvent = default;

        private bool obtained = false;

        public void Obtain()
        {
            if (!obtained)
            {
                obtained = true;
                StartCoroutine(RewardCoroutine(amountObtain));
            }
        }

        private IEnumerator RewardCoroutine(int amount)
        {
            yield return rewardEvent?.Raise(new RewardData(0, amount, null, null, null, null, null, null, false));
            hcObtainedEvent?.Raise(amount);
        }
    }
}