using GamedevsToolbox.StateMachine;

namespace GamedevsToolbox.UISolution
{
    [System.Serializable]
    public class MenuStateMachine : CoroutineStateMachine
    {
        public override void ReceiveSignal(string signal)
        {
            currentState.ReceiveSignal(signal);
        }
    }
}