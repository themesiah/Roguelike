using System;
using System.Linq;

namespace GamedevsToolbox.Examples
{
    public class NestedStateMachineExample : StateMachine.FiniteStateMachine
    {
        public override void EnterState()
        {
            if (currentState != null)
            {
                currentState.EnterState();
            }
        }

        public override void ExitState()
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
        }

        public override void ReceiveSignal(string signal)
        {
            currentState.ReceiveSignal(signal);
        }

        public override string Update()
        {
            if (currentState == null)
            {
                currentState = states.Values.First();
                currentState.EnterState();
            }
            string newState = currentState?.Update();
            if (newState != null)
            {
                if (states.ContainsKey(newState))
                {
                    currentState.ExitState();
                    currentState = states[newState];
                    currentState.EnterState();
                }
                else
                {
                    return newState;
                }
            }
            return null;
        }
    }
}