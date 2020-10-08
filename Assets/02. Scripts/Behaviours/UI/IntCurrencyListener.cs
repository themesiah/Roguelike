using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.UI;

namespace Laresistance.Behaviours
{
    public class IntCurrencyListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableIntReference currencyReference = default;

        [SerializeField]
        private Text currencyText = default;

        private void OnEnable()
        {
            currencyReference.RegisterOnChangeAction(ListenToValue);
            SetText();
        }

        private void OnDisable()
        {
            currencyReference.UnregisterOnChangeAction(ListenToValue);
        }

        private void Awake()
        {
            SetText();
        }

        private void SetText()
        {
            currencyText.text = currencyReference.GetValue().ToString();
        }

        private void ListenToValue(int newValue)
        {
            SetText();
        }
    }
}