using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamedevsToolbox.StateMachine;

namespace GamedevsToolbox.Examples
{
    public class StateMachineControllerExample : MonoBehaviour
    {
        private FiniteStateMachine fsm = new StateMachineExample();

        private void Start()
        {
            NestedStateMachineExample nestedFSM = new NestedStateMachineExample();
            nestedFSM.SetStates(new Dictionary<string, IState> {
                { "Example4", new StateExample4() },
                { "Example5", new StateExample5() }
            });

            fsm.SetStates(new Dictionary<string, IState> {
                { "Example1", new StateExample1() },
                { "Example2", new StateExample2() },
                { "Example3", new StateExample3() },
                { "Nested", nestedFSM }
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                fsm.ReceiveSignal("ToState1");
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                fsm.ReceiveSignal("ToState2");
            } else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                fsm.ReceiveSignal("ToState3");
            } else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                fsm.ReceiveSignal("ToState4");
            } else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                fsm.ReceiveSignal("ToState5");
            }
            fsm.Update();
        }
    }
}