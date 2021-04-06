using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    public abstract class RuntimeSingle<T> : ScriptableObject
    {
        [SerializeField]
        private T Item;

        private UnityAction<T> onSetEvent = delegate { };

        public void Set(T thing)
        {
            Item = thing;
            onSetEvent?.Invoke(thing);
        }

        public T Get()
        {
            return Item;
        }

        public void RegisterOnSetEvent(UnityAction<T> action)
        {
            onSetEvent += action;
        }

        public void UnregisterOnSetEvent(UnityAction<T> action)
        {
            onSetEvent -= action;
        }
    }
}