using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamedevsToolbox.StateMachine
{
    public class FiniteStateMachine : IState
    {
        protected IState currentState = null;
        protected Dictionary<string, IState> states;

        public void SetStates(Dictionary<string, IState> states)
        {
            this.states = states;
        }

        public virtual void EnterState() { }

        public virtual void ExitState() { }

        public virtual void ReceiveSignal(string signal) { }

        public virtual string Update()
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
                } else
                {
                    Debug.LogError("Your state machine of type " + this.GetType().Name + " is trying to switch to the state " + newState + " which is not in the State Machine dictionary");
                }
            }
            return null;
        }
    }
}
