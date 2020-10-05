using System;
using System.Collections;
using GamedevsToolbox.StateMachine;
using Laresistance.Behaviours;

namespace Laresistance.StateMachines
{
    public class GameManagerMapState : ICoroutineState
    {
        private string nextState = null;

        public IEnumerator EnterState()
        {
            yield return GameSceneManager.Instance.SceneLoadAndChange("Scenario2");
        }

        public IEnumerator ExitState()
        {
            nextState = null;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            nextState = signal;
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (nextState != null)
            {
                resolve(nextState);
            }
            yield return null;
        }
    }
}