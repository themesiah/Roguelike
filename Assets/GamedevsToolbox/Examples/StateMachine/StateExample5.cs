using GamedevsToolbox.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamedevsToolbox.Examples
{
    public class StateExample5 : IState
    {
        private bool signalReceived = false;
        public void EnterState()
        {
            Debug.Log("Entered on state 5");
        }

        public void ExitState()
        {
            Debug.Log("Exited state 5");
        }

        public void ReceiveSignal(string signal)
        {
            if (signal == "ToState1")
            {
                signalReceived = true;
            }
        }

        public string Update()
        {
            if (signalReceived)
            {
                signalReceived = false;
                return "Example1";
            }
            return null;
        }
    }
}