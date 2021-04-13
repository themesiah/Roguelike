using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Laresistance.TestAndDebug
{
    public class DebugInputAction : MonoBehaviour
    {
        [SerializeField]
        private InputAction action = default;
        [SerializeField]
        private UnityEvent onActionTriggered = default;

        private void Start()
        {
            action?.Enable();
            action.performed += OnPerformed;
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            onActionTriggered?.Invoke();
        }

        private void OnDestroy()
        {
            action?.Disable();
        }
    }
}