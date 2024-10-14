namespace AXitUnityTemplate.StateMachine.Signal
{
    using AXitUnityTemplate.StateMachine.Interface;

    public class OnStateExitSignal
    {
        public IState State { get; }

        public OnStateExitSignal(IState state) { this.State = state; }
    }
}