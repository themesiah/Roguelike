using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class StateExample3 : IState
    {
        private bool signalReceived = false;
        private bool signalReceived2 = false;
        public void EnterState()
        {
            Debug.Log("Entered on state 3");
        }

        public void ExitState()
        {
            Debug.Log("Exited state 3");
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "ToState1")
            {
                signalReceived = true;
            }
            if (signal == "ToState4")
            {
                signalReceived2 = true;
            }
        }

        public string Update()
        {
            if (signalReceived)
            {
                signalReceived = false;
                return "Example1";
            }
            if (signalReceived2)
            {
                signalReceived2 = false;
                return "Nested";
            }
            return null;
        }
    }
}