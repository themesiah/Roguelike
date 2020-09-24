using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnEnableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onEnableEvent = default;

        private void OnEnable()
        {
            onEnableEvent.Invoke();
        }
    }
}