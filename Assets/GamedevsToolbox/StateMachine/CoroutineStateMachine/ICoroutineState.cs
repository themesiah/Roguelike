using System.Collections;
using System;
using GamedevsToolbox.Utils;

namespace GamedevsToolbox.StateMachine
{
    public interface ICoroutineState : IPausable
    {
        IEnumerator EnterState();
        IEnumerator ExitState();
        IEnumerator Update(Action<string> resolve);
        void ReceiveSignal(string signal);
    }
}