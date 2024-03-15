namespace UITemplate.Scripts.Extension.StateMachine.Interface
{
    using StateMachines;

    public interface IHaveStateMachine
    {
        GameStateMachine StateMachine { get; set; }
    }
}