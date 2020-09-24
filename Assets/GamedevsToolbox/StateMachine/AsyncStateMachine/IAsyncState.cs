using System;
using System.Threading.Tasks;

namespace GamedevsToolbox.StateMachine
{
    public interface IAsyncState
    {
        Task EnterState();
        Task<string> Update();
        Task ExitState();
        void ReceiveSignal(string signal);
    }
}