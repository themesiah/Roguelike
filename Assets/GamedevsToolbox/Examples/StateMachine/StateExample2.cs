using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class StateExample2 : IState
    {
        private bool signalReceived = false;
        public void EnterState()
        {
            Debug.Log("Entered on state 2");
        }

        public void ExitState()
        {
            Debug.Log("Exited state 2");
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "ToState3")
            {
                signalReceived = true;
            }
        }

        public string Update()
        {
            if (signalReceived)
            {
                signalReceived = false;
                return "Example3";
            }
            return null;
        }
    }
}