namespace AXitUnityTemplate.Scripts
{
    using Zenject;
    using AXitUnityTemplate.UserData;
    using AXitUnityTemplate.GameAssets;
    using AXitUnityTemplate.ObjectPool;
    using AXitUnityTemplate.StateMachine;
    using AXitUnityTemplate.ScreenTemplate.Scripts;
    using AXitUnityTemplate.Blueprint.BlueprintControlFlow;

    public class UnityTemplateInstall : Installer<UnityTemplateInstall>
    {
        public override void InstallBindings()
        {
            // Default project install
            SignalBusInstaller.Install(this.Container);
            StateMachineInstall.Install(this.Container);
            GameAssetInstall.Install(this.Container);
            ObjectPoolInstall.Install(this.Container);
            ScreenTemplateInstall.Install(this.Container);
            BlueprintServicesInstaller.Install(this.Container);
            UserDataInstall.Install(this.Container);
        }
    }
}