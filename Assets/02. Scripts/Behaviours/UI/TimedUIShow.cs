using System.Collections;
using UnityEngine;

namespace Laresistance.Behaviours
{
    public class TimedUIShow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup = default;

        [SerializeField]
        private float showTime = 1.5f;

        [SerializeField]
        private float alphaTime = 0.5f;

        public void Show()
        {
            canvasGroup.alpha = 1f;
            StartCoroutine(Hide());
        }

        private IEnumerator Hide()
        {
            yield return new WaitForSeconds(showTime);
            float timer = 0f;
            while(timer < alphaTime)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = (1f - timer / alphaTime);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }
    }
}