using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;

namespace Laresistance.StateMachines
{
    public class GameContextUIState : ICoroutineState
    {
        private bool returnToMap = false;
        private StringGameEvent actionMapSwitchEvent;

        public GameContextUIState(StringGameEvent actionMapSwitchEvent)
        {
            this.actionMapSwitchEvent = actionMapSwitchEvent;
        }

        public IEnumerator EnterState()
        {
            GamedevsToolbox.Utils.Logger.Logger.Log("Entering UI State");
            actionMapSwitchEvent.Raise("UI");
            yield return null;
        }

        public IEnumerator ExitState()
        {
            returnToMap = false;
            yield return null;
        }

        public void Pause()
        {
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "Map")
                returnToMap = true;
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (returnToMap)
            {
                resolve("Map");
            }
            yield return null;
        }
    }
}