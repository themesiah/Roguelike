using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;
using Laresistance.Systems;
using GamedevsToolbox.ScriptableArchitecture.Values;
using Laresistance.Data;

namespace Laresistance.Behaviours
{
    public class BreakableUrn : MonoBehaviour, IBreakable
    {
        [SerializeField]
        private GameObject fullSpriteObject = default;
        [SerializeField]
        private GameObject partSpriteObject = default;
        [SerializeField]
        private SpriteRenderer[] renderers = default;

        [SerializeField]
        private float fadeDuration = 1f;
		
		[Header("References")]
        [SerializeField]
        private RuntimePlayerDataBehaviourSingle playerDataRef = default;
        [SerializeField]
        private ScriptableIntReference bloodReference = default;
        [SerializeField]
        private ScriptableIntReference hardCurrencyReference = default;
		
		private RewardSystem rewardSystem;
		private bool alreadyBroke = false;

        private void Start()
        {
			rewardSystem = new RewardSystem(playerDataRef.Get().player, bloodReference, hardCurrencyReference, null);
        }

        public void Break()
        {
			if (!alreadyBroke) {
				fullSpriteObject.SetActive(false);
				partSpriteObject.SetActive(true);
				// Give resources here
				TweenFactory.Tween(gameObject.name, 1f, 0f, fadeDuration, TweenScaleFunctions.CubicEaseIn, UpdateAlpha, FadeCompleted);
				StartCoroutine(RewardCoroutine());
			}
        }
		
		private IEnumerator RewardCoroutine()
		{
			int amount = 1;
			yield return rewardSystem.GetReward(new RewardData(amount, 0, null, null, null, null, null));
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