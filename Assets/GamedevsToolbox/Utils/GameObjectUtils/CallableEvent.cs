using UnityEngine;
using UnityEngine.Events;

namespace GamedevsToolbox.Utils
{
    public class CallableEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent callableEvent = default;

        public void CallEvent()
        {
            callableEvent?.Invoke();
        }
    }
}