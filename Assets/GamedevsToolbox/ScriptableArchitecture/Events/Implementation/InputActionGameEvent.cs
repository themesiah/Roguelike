using UnityEngine;
using UnityEngine.InputSystem;

namespace GamedevsToolbox.ScriptableArchitecture.Events
{
    [CreateAssetMenu(menuName = "Scriptable Architecture/Events/Input action game event")]
    public class InputActionGameEvent : TemplatedGameEvent<InputAction.CallbackContext>
    {
    }
}