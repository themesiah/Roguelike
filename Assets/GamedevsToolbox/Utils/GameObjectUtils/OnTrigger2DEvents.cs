using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Collider2D))]
    public class OnTrigger2DEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onTriggerEnterEvent = default;

        [SerializeField]
        private UnityEvent onTriggerExitEvent = default;

        [SerializeField]
        private string tagToCheck = null;

        [SerializeField]
        private bool checkTag = false;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                onTriggerEnterEvent.Invoke();
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                onTriggerExitEvent.Invoke();
            }
        }
    }
}