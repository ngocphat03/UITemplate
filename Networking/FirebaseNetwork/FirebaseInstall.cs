namespace AXitUnityTemplate.Networking.FirebaseNetwork
{
    using Zenject;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Signals;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Database;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication;
    using AXitUnityTemplate.Networking.FirebaseNetwork.RealtimeDatabase;

    public class FirebaseInstall : Installer<FirebaseInstall>
    {
        public override void InstallBindings()
        {
            this.DeclareSignal();
            
            AuthenticationInstall.Install(this.Container);
            RealtimeDatabaseInstall.Install(this.Container);
            DatabaseInstall.Install(this.Container);
        }
        
        private void DeclareSignal()
        {
            this.Container.DeclareSignal<FirebaseAuthenticationInitializedSignal>();
        }
    }
}