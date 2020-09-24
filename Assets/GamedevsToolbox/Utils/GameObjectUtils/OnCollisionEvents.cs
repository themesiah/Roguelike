using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Collider))]
    public class OnCollisionEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onCollisionEnterEvent = default;

        [SerializeField]
        private UnityEvent onCollisionExitEvent = default;

        [SerializeField]
        private string tagToCheck = null;

        [SerializeField]
        private bool checkTag = false;

        public void OnCollisionEnter(Collision other)
        {
            if (!checkTag || other.gameObject.tag == tagToCheck)
            {
                onCollisionEnterEvent.Invoke();
            }
        }

        public void OnCollisionExit(Collision other)
        {
            if (!checkTag || other.gameObject.tag == tagToCheck)
            {
                onCollisionExitEvent.Invoke();
            }
        }
    }
}