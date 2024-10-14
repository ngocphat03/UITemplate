namespace AXitUnityTemplate.StateMachine.Signal
{
    using AXitUnityTemplate.StateMachine.Interface;

    public class OnStateEnterSignal
    {
        public IState State { get; }

        public OnStateEnterSignal(IState state) { this.State = state; }
    }
}