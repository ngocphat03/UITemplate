namespace AXitUnityTemplate.StateMachine.Runtime.Interfaces
{
    public interface IState
    {
        public void Enter();

        public void Update();
        
        public void Exit();
    }
    
    public interface IState<in TModel> : IState
    {
        public TModel Model { set; }
    }
}
