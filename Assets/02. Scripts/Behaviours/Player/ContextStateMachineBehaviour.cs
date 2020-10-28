using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class GameManagerStateMachineBehaviour : MonoBehaviour
    {
        private SimpleSignalStateMachine stateMachine = default;

        void Start()
        {
            stateMachine = new SimpleSignalStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("MainMenu", new GameManagerMainMenuState());
            states.Add("Map", new GameManagerMapState());

            stateMachine.SetStates(states);
            StartCoroutine(StateMachineCoroutine());
        }

        public void ReceiveSignal(string signal)
        {
            stateMachine.ReceiveSignal(signal);
        }

        IEnumerator StateMachineCoroutine()
        {
            while (true)
            {
                yield return stateMachine.Update(null);
            }
        }
    }
}