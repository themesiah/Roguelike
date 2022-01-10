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

        [Header("References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private SpriteRenderer spriteRenderer = default;
        [SerializeField]
        private IntGameEvent bloodObtainedEvent = default;

        private int currentLevel = 1;

        public void SetCurrentLevel(int currentLevel)
        {
            this.currentLevel = currentLevel;
        }

        public void Obtain()
        {
            int amount = GetBloodAmount();
            StartCoroutine(RewardCoroutine(amount));
        }

        private int GetBloodAmount()
        {
            return baseBloodAmount.GetValue() * currentLevel;
        }

        private IEnumerator RewardCoroutine(int amount)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;
            yield return rewardEvent?.Raise(new RewardData(amount, 0, null, null, null, null, null, null, false));
            bloodObtainedEvent?.Raise(amount);
            Destroy(gameObject);
        }
    }
}