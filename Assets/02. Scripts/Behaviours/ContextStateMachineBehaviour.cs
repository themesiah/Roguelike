using UnityEngine;
using GamedevsToolbox.StateMachine;
using Laresistance.StateMachines;
using System.Collections.Generic;
using System.Collections;

namespace Laresistance.Behaviours
{
    public class ContextStateMachineBehaviour : MonoBehaviour
    {
        private GameContextStateMachine stateMachine = default;

        void Start()
        {
            stateMachine = new GameContextStateMachine();
            Dictionary<string, ICoroutineState> states = new Dictionary<string, ICoroutineState>();
            states.Add("MainMenu", new MainMenuContextState());
            states.Add("Map", new MapContextState());

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