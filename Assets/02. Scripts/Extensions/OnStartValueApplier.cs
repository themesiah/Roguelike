using UnityEngine;
using UnityEngine.Events;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Extensions
{
    public class OnStartValueApplier<T,T2,T3> : MonoBehaviour where T :  System.IEquatable<T> where T2 : ScriptableValue<T> where T3 : ScriptableReference<T,T2>
    {
        [SerializeField]
        private UnityEvent<T> OnStart = default;
        [SerializeField]
        private T3 reference = default;

        protected virtual void Start()
        {
            OnStart?.Invoke(reference.GetValue());
        }
    }
}