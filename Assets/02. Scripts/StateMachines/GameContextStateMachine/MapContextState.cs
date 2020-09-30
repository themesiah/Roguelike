using System;
using System.Collections;
using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;

namespace Laresistance.StateMachines
{
    public class MapContextState : ICoroutineState
    {
        private string nextScene = null;

        public IEnumerator EnterState()
        {
            yield return GameSceneManager.Instance.SceneLoadAndChange("Scenario2");
        }

        public IEnumerator ExitState()
        {
            nextScene = null;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            nextScene = signal;
        }

        public void Resume()
        {
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