using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    [RequireComponent(typeof(Collider2D))]
    public class OnTrigger2DEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Collider2D> onTriggerEnterEvent = default;

        [SerializeField]
        private UnityEvent<Collider2D> onTriggerExitEvent = default;

        [SerializeField]
        private string tagToCheck = null;

        [SerializeField]
        private bool checkTag = false;

        [SerializeField]
        private bool destroyOnTrigger = false;

        [SerializeField]
        private bool logEvents = false;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                Log("OnTriggerEnter2D");
                onTriggerEnterEvent.Invoke(other);
                if (destroyOnTrigger)
                {
                    Destroy(this);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!checkTag || other.tag == tagToCheck)
            {
                Log("OnTriggerExit2D");
                onTriggerExitEvent.Invoke(other);
                if (destroyOnTrigger)
                {
                    Destroy(this);
                }
            }
        }

        private void Log(string text)
        {
            if (logEvents)
            {
                if (checkTag)
                {
                    Debug.LogFormat("[OnTrigger2DEvents@{0}] [W/Tag {1}] {2}", name, tagToCheck, text);
                }
                else
                {
                    Debug.LogFormat("[OnTrigger2DEvents@{0}] {1}", name, text);
                }
            }
        }
    }
}