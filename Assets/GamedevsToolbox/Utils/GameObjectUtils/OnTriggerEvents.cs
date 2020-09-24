using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Collider))]
    public class OnTriggerEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onTriggerEnterEvent = default;

        [SerializeField]
        private UnityEvent onTriggerExitEvent = default;

        [SerializeField]
        private string tagToCheck = null;

        [SerializeField]
        private bool checkTag = false;

        public void OnTriggerEnter(Collider other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                onTriggerEnterEvent.Invoke();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                onTriggerExitEvent.Invoke();
            }
        }
    }
}