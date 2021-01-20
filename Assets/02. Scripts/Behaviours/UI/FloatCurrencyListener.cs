using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class FloatCurrencyListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference currencyReference = default;

        [SerializeField]
        private UnityEvent<float> OnValueChanged = default;
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

        private void ListenToValue(float newValue)
        {
            OnValueChanged?.Invoke(newValue);
            OnValueChangedString?.Invoke(newValue.ToString());
        }
    }
}