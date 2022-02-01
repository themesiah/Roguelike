using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    [System.Serializable]
    public class ScriptableValue<T> : ScriptableObject where T : System.IEquatable<T>
    {
        [SerializeField]
        private T value = default;

        [SerializeField]
        [Tooltip("A default value may come in handy to reset it at the start of a level, for example.")]
        private T defaultValue = default;

        [SerializeField]
        private bool resetOnAwake = false;

        private T lastValue;
        private UnityAction<T> onValueChangedEvent = delegate { };

        private void Awake()
        {
            if (resetOnAwake)
            {
                Debug.LogFormat("ScriptableValue {0} reseted on awake", name);
                ResetValue();
            }
        }

        #region IScriptableValue implementation
        public virtual T GetValue()
        {
            return value;
        }

        public void SetValue(T value)
        {
            lastValue = this.value;
            this.value = value;
            InvokeOnChangeAction();
        }

        public void ResetValue()
        {
            SetValue(defaultValue);
        }

        public void RegisterOnChangeAction(UnityAction<T> action)
        {
            onValueChangedEvent += action;
        }

        public void UnregisterOnChangeAction(UnityAction<T> action)
        {
            onValueChangedEvent -= action;
        }
        #endregion

        #region Private Methods
        private void InvokeOnChangeAction()
        {
            if (onValueChangedEvent != null && !lastValue.Equals(value))
            {
                onValueChangedEvent.Invoke(value);
            }
        }
        #endregion
    }
}