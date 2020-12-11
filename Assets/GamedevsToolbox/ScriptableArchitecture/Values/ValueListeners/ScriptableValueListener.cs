using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    public class ScriptableValueListener<T,T2,T3> : MonoBehaviour where T : System.IEquatable<T> where T2 : ScriptableValue<T> where T3 : ScriptableReference<T,T2>
    {
        [SerializeField]
        private T3 valueReference = default;
        [SerializeField]
        private UnityEvent<T> OnValueChangedEvent = default;

        private void Start()
        {
            OnValueChanged(valueReference.GetValue());
        }

        private void OnEnable()
        {
            valueReference.RegisterOnChangeAction(OnValueChanged);
        }

        private void OnDisable()
        {
            valueReference.UnregisterOnChangeAction(OnValueChanged);
        }

        private void OnValueChanged(T newValue)
        {
            OnValueChangedEvent?.Invoke(newValue);
        }
    }
}