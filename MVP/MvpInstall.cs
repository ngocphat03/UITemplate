namespace AXitUnityTemplate.MVP
{
    using AXitUnityTemplate.MVP.Core;
    using VContainer;

#if ZENJECT
    using Zenject;

    public class MvpInstall : Installer<MvpInstall>
    {
        public override void InstallBindings() { this.Container.Bind<MvpFactory>().AsSingle().NonLazy(); }
    }
#elif VCONTAINER
    public class MvpInstall
    {
        public static void Install(IContainerBuilder builder) { builder.Register<MvpFactory>(Lifetime.Singleton); }
    }
#else
#endif
}