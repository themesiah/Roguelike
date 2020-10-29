using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laresistance.Behaviours
{
    public class PauseBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameEvent pauseEvent = default;
        [SerializeField]
        private GameEvent resumeEvent = default;

        private bool paused = false;

        public void TogglePause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (paused)
                {
                    // Resume
                    resumeEvent.Raise();
                    paused = false;
                }
                else
                {
                    // Pause
                    pauseEvent.Raise();
                    paused = true;
                }
            }
        }
    }
}