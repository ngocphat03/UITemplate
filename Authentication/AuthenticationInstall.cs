#if FIREBASE && AUTHENTICATION
namespace UITemplate.Authentication
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