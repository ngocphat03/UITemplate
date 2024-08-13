namespace AXitUnityTemplate.Networking.Firebase
{
    using Zenject;
    using AXitUnityTemplate.Networking.Firebase.Database;
    using AXitUnityTemplate.Networking.Firebase.Authentication;

    public class FirebaseInstall : Installer<FirebaseInstall>
    {
        public override void InstallBindings()
        {
            AuthenticationInstall.Install(this.Container);
            DatabaseInstall.Install(this.Container);
        }
    }
}