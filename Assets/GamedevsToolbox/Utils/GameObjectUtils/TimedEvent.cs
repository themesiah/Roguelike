using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace GamedevsToolbox.Utils
{
    public class TimedEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent timedEvent = default;
        [SerializeField]
        private float delay = 1f;

        private void Start()
        {
            StartCoroutine(TimedEventCoroutine());
        }

        private IEnumerator TimedEventCoroutine()
        {
            yield return new WaitForSeconds(delay);
            timedEvent?.Invoke();
        }
    }
}
