using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnDisableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDisableEvent = default;

        private void OnDisable()
        {
            onDisableEvent.Invoke();
        }
    }
}