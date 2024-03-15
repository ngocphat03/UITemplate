namespace UITemplate.Scripts.Extension.StateMachine.Signal
{
    using UITemplate.Scripts.Extension.StateMachine.Interface;

    public class OnStateExitSignal
    {
        public IState State { get; }

        public OnStateExitSignal(IState state) { this.State = state; }
    }
}