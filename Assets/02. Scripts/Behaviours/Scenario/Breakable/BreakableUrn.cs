using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

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
        private Vector2 breakForceMinMax = default;
        [SerializeField]
        private PointEffector2D explosionEffect = default;

        [SerializeField]
        private float fadeDuration = 1f;

        private void Start()
        {
            //explosionEffect.forceMagnitude = Random.Range(breakForceMinMax.x, breakForceMinMax.y);
        }

        public void Break()
        {
            fullSpriteObject.SetActive(false);
            partSpriteObject.SetActive(true);
            // Give resources here
            TweenFactory.Tween(gameObject.name, 1f, 0f, fadeDuration, TweenScaleFunctions.CubicEaseIn, UpdateAlpha, FadeCompleted);
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