namespace AXitUnityTemplate.Networking.Firebase.Database
{
    using Zenject;
    using AXitUnityTemplate.Networking.Firebase.Database.UserProfile;

    public class DatabaseInstall : Installer<DatabaseInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UserProfileManager>().AsSingle().NonLazy();
        }
    }
}