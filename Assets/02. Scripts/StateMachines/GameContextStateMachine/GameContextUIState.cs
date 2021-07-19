using GamedevsToolbox.ScriptableArchitecture.Events;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using UnityEngine;

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
            Debug.LogFormat("Received signal {0} on UI State", signal);
            if (signal == "Map")
            {
                returnToMap = true;
            }
            else
            {
                returnToMap = false;
            }
        }

        public void Resume()
        {
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (returnToMap)
            {
                Debug.LogFormat("Resolving signal {0} on UI State", "Map");
                resolve("Map");
            }
            else
            {
                yield return null;
            }
        }
    }
}