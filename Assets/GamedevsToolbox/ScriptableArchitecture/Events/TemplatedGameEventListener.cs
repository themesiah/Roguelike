using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    public abstract class TemplatedGameEventListener<T> : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        [SerializeField]
        public TemplatedGameEvent<T> Event;

        [System.Serializable]
        public class ObjectEvent : UnityEvent<T> { };

        [Tooltip("Response to invoke when Event is raised.")]
        [SerializeField]
        public UnityEvent<T> Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public virtual void OnEventRaised(T data)
        {
            Response.Invoke(data);
        }
    }
}
