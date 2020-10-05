using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Destroy the listener when the event is triggered. Useful for events that must be triggered one time.")]
        private bool destroyOnEvent = false;

        [SerializeField]
        [Tooltip("Event to register with.")]
        private GameEvent Event = default;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")]
        private UnityEvent Response = default;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public virtual void OnEventRaised()
        {
            Response.Invoke();
            if (destroyOnEvent)
            {
                Destroy(this);
            }
        }
    }
}