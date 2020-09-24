using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class StateExample1 : IState
    {
        private bool signalReceived = false;
        public void EnterState()
        {
            Debug.Log("Entered on state 1");
        }

        public void ExitState()
        {
            Debug.Log("Exited state 1");
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "ToState2")
            {
                signalReceived = true;
            }
        }

        public string Update()
        {
            if (signalReceived)
            {
                signalReceived = false;
                return "Example2";
            }
            return null;
        }
    }
}