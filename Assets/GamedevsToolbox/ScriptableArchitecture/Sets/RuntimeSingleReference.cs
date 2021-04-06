using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Sets
{
    public class RuntimeSingleReference<T, T2> : MonoBehaviour where T2 : RuntimeSingle<T>
    {
        [SerializeField]
        private T2 singleObject = default;
        [SerializeField]
        private UnityEvent<T> referenceEvent = default;

        private void OnEnable()
        {
            singleObject.RegisterOnSetEvent(OnReferenceSet);
            if (singleObject.Get() != null)
            {
                OnReferenceSet(singleObject.Get());
            }
        }

        private void OnDisable()
        {
            singleObject.UnregisterOnSetEvent(OnReferenceSet);
        }

        private void OnReferenceSet(T obj)
        {
            referenceEvent?.Invoke(obj);
        }
    }
}