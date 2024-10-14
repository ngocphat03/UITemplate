namespace AXitUnityTemplate.StateMachine
{
    using Zenject;
    using AXitUnityTemplate.StateMachine.Signal;

    public class StateMachineInstall : Installer<StateMachineInstall>
    {
        public override void InstallBindings()
        {
            this.Container.DeclareSignal<OnStateEnterSignal>();
            this.Container.DeclareSignal<OnStateExitSignal>();
        }
    }
}