using UnityEngine;
using UnityEngine.UI;
using GamedevsToolbox.ScriptableArchitecture.Values;

namespace Laresistance.Behaviours
{
    public class HealthTextManualBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Text healthText = default;

        [SerializeField]
        private string healthTemplate = "{0} / {1}";

        int health = 0;
        int maxHealth = 0;

        public void SetHealth(int val)
        {
            health = val;
            UpdateText();
        }

        public void SetMaxHealth(int val)
        {
            maxHealth = val;
            UpdateText();
        }

        private void UpdateText()
        {
            healthText.text = string.Format(healthTemplate, health.ToString(), maxHealth.ToString());
        }
    }
}
