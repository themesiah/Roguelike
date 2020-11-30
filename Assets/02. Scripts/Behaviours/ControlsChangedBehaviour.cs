using GamedevsToolbox.ScriptableArchitecture.Values;
using GamedevsToolbox.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    [RequireComponent(typeof(PlayerInput))]
    public class ControlsChangedBehaviour : MonoBehaviour, IPausable
    {
        [SerializeField]
        private PlayerInput playerInput = default;

        [SerializeField]
        private ScriptableIntReference currentSchemeReference = default;

        private string currentControlScheme = null;
        private string lastActionMap = null;

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

        public void ActionMapChanged(string newActionMap)
        {
            lastActionMap = newActionMap;
        }

        public void Pause()
        {
            playerInput.SwitchCurrentActionMap("UI");
        }

        public void Resume()
        {
            playerInput.SwitchCurrentActionMap(lastActionMap);
        }
    }
}