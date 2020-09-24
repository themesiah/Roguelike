using GamedevsToolbox.StateMachine;

namespace GamedevsToolbox.UISolution
{
    [System.Serializable]
    public class AsyncMenuStateMachine : AsyncStateMachine
    {
        public override void ReceiveSignal(string signal)
        {
            currentState.ReceiveSignal(signal);
        }
    }
}