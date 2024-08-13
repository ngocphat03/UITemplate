#if FIREBASE && AUTHENTICATION
namespace AXitUnityTemplate.Networking.Firebase.Authentication
{
    using Zenject;

    public class AuthenticationInstall : Installer<AuthenticationInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<AuthenticationService>().AsSingle().NonLazy();
        }
    }
}
#endif