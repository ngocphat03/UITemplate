#if FIREBASE && AUTHENTICATION
namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication
{
    using Zenject;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Providers.GoogleAuth;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Providers.FacebookAuth;

    public class AuthenticationInstall : Installer<AuthenticationInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<AuthenticationService>().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<GoogleAuthHandler>().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<FacebookAuthHandler>().AsSingle().NonLazy();
        }
    }
}
#endif