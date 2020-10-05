using System;
using System.Collections;
using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;

namespace Laresistance.StateMachines
{
    public class GameManagerMainMenuState : ICoroutineState
    {
        private string nextScene = null;

        public IEnumerator EnterState()
        {
            yield return GameSceneManager.Instance.SceneLoadAndChange("MainMenu");
        }

        public IEnumerator ExitState()
        {
            nextScene = null;
            yield return null;
        }

        public void Pause()
        {
            // Nothing
        }

        public void ReceiveSignal(string signal)
        {
            nextScene = signal;
        }

        public void Resume()
        {
            // Nothing
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (nextScene != null)
            {
                resolve(nextScene);
            }
            yield return null;
        }
    }
}