using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnStartEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onStartEvent = default;

        private void Start()
        {
            onStartEvent.Invoke();
        }
    }
}