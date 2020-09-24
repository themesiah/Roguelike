using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

namespace GamedevsToolbox.StateMachine
{
    public class AsyncStateMachine : IAsyncState
    {
        protected IAsyncState currentState = null;
        protected Dictionary<string, IAsyncState> states;

        public void SetStates(Dictionary<string, IAsyncState> states)
        {
            this.states = states;
        }

        public virtual Task EnterState() {
            return null;
        }

        public virtual Task ExitState() {
            return null;
        }

        public virtual void ReceiveSignal(string signal) { }

        async public virtual Task<string> Update()
        {
            if (currentState == null)
            {
                currentState = states.Values.First();
                await currentState.EnterState();
            }
            string newState = await currentState?.Update();
            if (newState != null)
            {
                if (states.ContainsKey(newState))
                {
                    await currentState.ExitState();
                    currentState = states[newState];
                    await currentState.EnterState();
                } else
                {
                    Debug.LogError("Your state machine of type " + this.GetType().Name + " is trying to switch to the state " + newState + " which is not in the State Machine dictionary");
                }
            }
            return null;
        }
    }
}
