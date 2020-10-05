using UnityEngine;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DamageTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        private float timeToDisappear = 1f;

        [SerializeField]
        private CanvasGroup canvasGroup = default;

        private float timer = 0f;

        private void Awake()
        {
            Destroy(gameObject, timeToDisappear);
        }

        private void Update()
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer/timeToDisappear);
        }
    }
}