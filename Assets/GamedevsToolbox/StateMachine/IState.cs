namespace GamedevsToolbox.StateMachine
{
    public interface IState
    {
        void EnterState();
        string Update();
        void ExitState();
        void ReceiveSignal(string signal);
    }
}