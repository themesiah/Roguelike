using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [System.Serializable]
    public class TemplatedStandaloneGameEventListener<T> : IGameEventListener<T>
    {
        [Tooltip("Event to register with.")]
        [SerializeField]
        public TemplatedGameEvent<T> Event;

        [System.Serializable]
        public class ObjectEvent : UnityEvent<T> { };

        [Tooltip("Response to invoke when Event is raised.")]
        [SerializeField]
        public UnityEvent<T> Response;

        public void Register()
        {
            Event.RegisterListener(this);
        }

        public void Unregister()
        {
            Event.UnregisterListener(this);
        }

        public virtual void OnEventRaised(T data)
        {
            Response.Invoke(data);
        }
    }
}