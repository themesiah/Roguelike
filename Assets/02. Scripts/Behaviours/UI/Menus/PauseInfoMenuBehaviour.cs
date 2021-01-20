using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.ScriptableArchitecture.Events;
using UnityEngine.Events;

namespace Laresistance.Behaviours
{
    public class PauseInfoMenuBehaviour : MonoBehaviour
    {
        [SerializeField]
        private BoolGameEvent pauseResumeGameStateChange = default;
        [SerializeField]
        private UnityEvent onPauseMenuOpen = default;
        [SerializeField]
        private UnityEvent onInfoMenuOpen = default;
        [SerializeField]
        private UnityEvent onPauseMenuClose = default;
        [SerializeField]
        private UnityEvent onInfoMenuClose = default;

        private bool pauseMenuOpened = false;
        private bool infoMenuOpened = false;
        private bool AnyMenuOpened { get { return pauseMenuOpened || infoMenuOpened; } }

        public void TogglePauseMenu()
        {
            if (!AnyMenuOpened)
            {
                pauseMenuOpened = true;
                pauseResumeGameStateChange?.Raise(true);
                onPauseMenuOpen?.Invoke();
            } else if (pauseMenuOpened)
            {
                pauseMenuOpened = false;
                pauseResumeGameStateChange?.Raise(false);
                onPauseMenuClose?.Invoke();
            }
        }

        public void ToggleInfoMenu()
        {
            if (!AnyMenuOpened)
            {
                infoMenuOpened = true;
                pauseResumeGameStateChange?.Raise(true);
                onInfoMenuOpen?.Invoke();
            } else if (infoMenuOpened)
            {
                infoMenuOpened = false;
                pauseResumeGameStateChange?.Raise(false);
                onInfoMenuClose?.Invoke();
            }
        }
    }
}