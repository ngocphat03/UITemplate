namespace AXitUnityTemplate.Networking.FirebaseNetwork.Database
{
    using AXitUnityTemplate.Networking.FirebaseNetwork.Database.UserProfile;
    using Zenject;

    public class DatabaseInstall : Installer<DatabaseInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UserProfileManager>().AsSingle().NonLazy();
        }
    }
}