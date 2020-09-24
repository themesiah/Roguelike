using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace GamedevsToolbox.ScriptableArchitecture.Values
{
    //[CreateAssetMenu(menuName = "Scriptable Architecture/Lists/Integer")]
    public class ScriptableList<T> : ScriptableObject where T : System.IEquatable<T>
    {
        [SerializeField]
        private List<T> values = default;

        [SerializeField]
        private ScriptableIntReference listIndex = default;

        private UnityAction<List<T>> onValueChangedEvent = delegate { };

        public T GetValue()
        {
            return values[listIndex.GetValue()];
        }

        public void SetValue(T value)
        {
            T last = values[listIndex.GetValue()];
            values[listIndex.GetValue()] = value;
            if (!value.Equals(last))
            {
                InvokeOnChangeAction();
            }
        }

        public void ResetValue()
        {
            values.Clear();
        }

        public void RegisterOnChangeAction(UnityAction<List<T>> action)
        {
            onValueChangedEvent += action;
        }

        public void UnregisterOnChangeAction(UnityAction<List<T>> action)
        {
            onValueChangedEvent -= action;
        }

        #region Private Methods
        private void InvokeOnChangeAction()
        {
            if (onValueChangedEvent != null)
            {
                onValueChangedEvent.Invoke(values);
            }
        }
        #endregion
    }
}