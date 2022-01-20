using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class PlayerFoundExclamation : MonoBehaviour
    {
        [SerializeField]
        private GameObject exclamationObject = default;
        [SerializeField]
        private AnimationCurve animationCurve = default;
        [SerializeField]
        private float animationDuration = default;
        [SerializeField]
        private float fadeDuration = default;
        [SerializeField]
        private Transform xPivotReference = default;

        public void DoAnimation()
        {
            StartCoroutine(AnimationCoroutine());
        }

        private IEnumerator AnimationCoroutine()
        {
            exclamationObject.SetActive(true);
            if (xPivotReference != null)
            {
                var pos = exclamationObject.transform.position;
                pos.x = xPivotReference.position.x;
                exclamationObject.transform.position = pos;
            }
            var scale = exclamationObject.transform.localScale;
            scale.y = 0f;
            exclamationObject.transform.localScale = scale;

            float timer = 0f;
            while (timer < animationDuration)
            {
                var t = timer / animationDuration;
                var scaleValue = animationCurve.Evaluate(t);
                scale.y = scaleValue;
                exclamationObject.transform.localScale = scale;
                timer += Time.deltaTime;
                yield return null;
            }
            scale.y = 1f;
            exclamationObject.transform.localScale = scale;

            yield return new WaitForSeconds(fadeDuration);

            exclamationObject.SetActive(false);
        }
    }
}