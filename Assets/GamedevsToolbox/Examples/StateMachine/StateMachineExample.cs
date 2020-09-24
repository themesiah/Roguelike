using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class StateMachineExample : StateMachine.FiniteStateMachine
    {
        public override void ReceiveSignal(string signal)
        {
            currentState.ReceiveSignal(signal);
        }
    }
}