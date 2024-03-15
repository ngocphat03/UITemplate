namespace UITemplate.Scripts.Extension.StateMachine.Signal
{
    using UITemplate.Scripts.Extension.StateMachine.Interface;

    public class OnStateEnterSignal
    {
        public IState State { get; }

        public OnStateEnterSignal(IState state) { this.State = state; }
    }
}