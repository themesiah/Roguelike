using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class HealthTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text healthText = default;

        [SerializeField]
        private string healthTemplate = "{0} / {1}";

        [SerializeField]
        private ScriptableIntReference currentHealthReference = default;
        [SerializeField]
        private ScriptableIntReference maxHealthReference = default;

        private void OnEnable()
        {
            currentHealthReference.RegisterOnChangeAction(SetText);
            maxHealthReference.RegisterOnChangeAction(SetText);
            SetText(0);
        }

        private void OnDisable()
        {
            currentHealthReference.UnregisterOnChangeAction(SetText);
            maxHealthReference.UnregisterOnChangeAction(SetText);
        }

        private void SetText(int val)
        {
            healthText.text = string.Format(healthTemplate, currentHealthReference.GetValue().ToString(), maxHealthReference.GetValue().ToString());
        }
    }
}
