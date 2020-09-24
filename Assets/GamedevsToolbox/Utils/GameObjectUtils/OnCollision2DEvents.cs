using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Collider2D))]
    public class OnCollision2DEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onCollisionEnterEvent = default;

        [SerializeField]
        private UnityEvent onCollisionExitEvent = default;

        [SerializeField]
        private string tagToCheck = null;

        [SerializeField]
        private bool checkTag = false;

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (!checkTag || other.gameObject.tag == tagToCheck)
            {
                onCollisionEnterEvent.Invoke();
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            if (!checkTag || other.gameObject.tag == tagToCheck)
            {
                onCollisionExitEvent.Invoke();
            }
        }
    }
}