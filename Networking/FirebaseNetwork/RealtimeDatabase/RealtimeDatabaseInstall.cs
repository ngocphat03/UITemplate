namespace AXitUnityTemplate.Networking.FirebaseNetwork.RealtimeDatabase
{
    using Zenject;

    public class RealtimeDatabaseInstall : Installer<RealtimeDatabaseInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<RealtimeDatabaseService>().AsSingle().NonLazy();
        }
    }
}