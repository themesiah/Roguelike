using GamedevsToolbox.ScriptableArchitecture.Values;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(PlayerInput))]
    public class ControlsChangedBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput = default;

        [SerializeField]
        private ScriptableIntReference currentSchemeReference = default;

        private string currentControlScheme = null;

        private void Awake()
        {
            currentControlScheme = playerInput.currentControlScheme;
        }

        private void Update()
        {
            if (playerInput.currentControlScheme != currentControlScheme)
            {
                Debug.LogFormat("Control scheme changed from {0} to {1}", currentControlScheme, playerInput.currentControlScheme);
                currentControlScheme = playerInput.currentControlScheme;
                switch(currentControlScheme)
                {
                    case "Gamepad":
                        currentSchemeReference.SetValue(1);
                        break;
                    case "Keyboard&Mouse":
                        currentSchemeReference.SetValue(0);
                        break;
                }
            }
        }
    }
}