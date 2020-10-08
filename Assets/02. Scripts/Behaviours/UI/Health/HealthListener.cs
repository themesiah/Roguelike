using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class HealthListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableFloatReference healthPercentReference = default;
        [SerializeField]
        private UnityEvent<float> onPercentHealthChanged = default;

        private void OnEnable()
        {
            healthPercentReference.RegisterOnChangeAction(onPercentHealthChanged.Invoke);
            onPercentHealthChanged?.Invoke(healthPercentReference.GetValue());
        }

        private void OnDisable()
        {
            healthPercentReference.UnregisterOnChangeAction(onPercentHealthChanged.Invoke);
        }
    }
}