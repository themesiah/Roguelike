using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class IntCurrencyListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableIntReference currencyReference = default;

        [SerializeField]
        private UnityEvent<int> OnValueChanged = default;
        [SerializeField]
        private UnityEvent<string> OnValueChangedString = default;

        private void OnEnable()
        {
            currencyReference.RegisterOnChangeAction(ListenToValue);
            ManualUpdate();
        }

        private void OnDisable()
        {
            currencyReference.UnregisterOnChangeAction(ListenToValue);
        }

        private void Awake()
        {
            ManualUpdate();
        }

        private void ManualUpdate()
        {
            ListenToValue(currencyReference.GetValue());
        }

        private void ListenToValue(int newValue)
        {
            OnValueChanged?.Invoke(newValue);
            OnValueChangedString?.Invoke(newValue.ToString());
        }
    }
}