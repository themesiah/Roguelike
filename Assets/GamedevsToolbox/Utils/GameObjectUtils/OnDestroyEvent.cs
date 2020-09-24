using UnityEngine.Events;
using UnityEngine;

namespace GamedevsToolbox.Utils {
    public class OnDestroyEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDestroyEvent = default;

        private void OnDestroy()
        {
            onDestroyEvent.Invoke();
        }
    }
}