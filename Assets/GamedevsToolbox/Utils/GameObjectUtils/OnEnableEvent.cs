using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnEnableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onEnableEvent = default;

        private void OnEnable()
        {
            Debug.LogFormat("Doing OnEnable of object {0}", gameObject.name);
            onEnableEvent.Invoke();
        }
    }
}