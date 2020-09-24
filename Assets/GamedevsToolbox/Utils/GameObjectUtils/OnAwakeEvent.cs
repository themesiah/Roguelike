using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnAwakeEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onAwakeEvent = default;

        private void Awake()
        {
            onAwakeEvent.Invoke();
        }
    }
}