using UnityEngine;
using DigitalRuby.Tween;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Events;
using Laresistance.Data;
using Laresistance.Audio;

namespace Laresistance.Behaviours
{
    public class BreakableUrn : MonoBehaviour, IBreakable
    {
        [Header("Object references")]
        [SerializeField]
        private GameObject fullSpriteObject = default;
        [SerializeField]
        private GameObject partSpriteObject = default;
        [SerializeField]
        private SpriteRenderer[] renderers = default;

        [Header("Effect configuration")]
        [SerializeField]
        private float fadeDuration = 1f;

        [Header("Reward configuration")]
        [SerializeField]
        private float bloodChance = 0.1f;
        [SerializeField]
        private Vector2Int bloodAmountRange = default;
        [SerializeField]
        private float hardCurrencyChance = 0.01f;
        [SerializeField]
        private Vector2Int hardCurrencyAmountRange = default;

        [Header("Other References")]
        [SerializeField]
        private RewardEvent rewardEvent = default;
        [SerializeField]
        private IntGameEvent bloodObtainedEvent = default;
        [SerializeField]
        private IntGameEvent hardCurrencyObtainedEvent = default;
        [SerializeField]
        private ScriptableFMODEventEmitter breakAudioEvent = default;

        private bool alreadyBroke = false;

        public void Break()
        {
			if (!alreadyBroke) {
				fullSpriteObject.SetActive(false);
				partSpriteObject.SetActive(true);
				// Give resources here
				TweenFactory.Tween(gameObject.name + gameObject.GetInstanceID().ToString(), 1f, 0f, fadeDuration, TweenScaleFunctions.CubicEaseIn, UpdateAlpha, FadeCompleted);
                breakAudioEvent.Play();
                GetReward();
                alreadyBroke = true;
            }
        }

        private void GetReward()
        {
            float value = Random.value;
            RewardData rd = null;
            int amount = 0;
            if (value < bloodChance)
            {
                amount = Random.Range(bloodAmountRange.x, bloodAmountRange.y);
                rd = new RewardData(amount, 0, null, null, null, null, null, null, false);
                bloodObtainedEvent?.Raise(amount);
            } else if (value < bloodChance+hardCurrencyChance)
            {
                amount = Random.Range(hardCurrencyAmountRange.x, hardCurrencyAmountRange.y);
                rd = new RewardData(0, amount, null, null, null, null, null, null, false);
                hardCurrencyObtainedEvent?.Raise(amount);
            }

            if (rd != null)
            {
                StartCoroutine(rewardEvent.Raise(rd));
            }
        }

        private void UpdateAlpha(ITween<float> t)
        {
            foreach(var renderer in renderers)
            {
                var color = renderer.color;
                color.a = t.CurrentValue;
                renderer.color = color;
            }
        }

        private void FadeCompleted(ITween<float> t)
        {
            Destroy(gameObject);
        }
    }
}