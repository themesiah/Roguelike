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

        public void GameStateChange(bool pausedSignal)
        {
            if (pausedSignal)
            {
                pauseEvent?.Raise();
            } else
            {
                resumeEvent?.Raise();
            }
        }
    }
}