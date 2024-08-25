namespace AXitUnityTemplate.Networking.FirebaseNetwork
{
    using Zenject;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Database;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication;

    public class FirebaseInstall : Installer<FirebaseInstall>
    {
        public override void InstallBindings()
        {
            AuthenticationInstall.Install(this.Container);
            DatabaseInstall.Install(this.Container);
        }
    }
}