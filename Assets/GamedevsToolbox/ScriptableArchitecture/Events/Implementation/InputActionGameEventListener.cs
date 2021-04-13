using UnityEngine;
using UnityEngine.InputSystem;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    public class InputActionGameEventListener : TemplatedGameEventListener<InputAction.CallbackContext>
    {
        [SerializeField]
        private bool eventOnlyOnPerformed = false;
        public override void OnEventRaised(InputAction.CallbackContext data)
        {
            if (eventOnlyOnPerformed == false)
            {
                Response?.Invoke(data);
            } else
            {
                if (data.performed)
                {
                    Response?.Invoke(data);
                }
            }
        }
    }
}
