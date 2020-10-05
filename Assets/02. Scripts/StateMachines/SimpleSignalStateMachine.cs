using GamedevsToolbox.StateMachine;

namespace Laresistance.StateMachines
{
    [System.Serializable]
    public class SimpleSignalStateMachine : CoroutineStateMachine
    {
        public override void ReceiveSignal(string signal)
        {
            currentState.ReceiveSignal(signal);
        }
    }
}