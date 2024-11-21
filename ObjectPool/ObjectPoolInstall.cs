namespace AXitUnityTemplate.ObjectPool
{
#if ZENJECT
    using Zenject;

    public class ObjectPoolInstall : Installer<ObjectPoolInstall>
    {
        public override void InstallBindings() { this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy(); }
    }

#elif VCONTAINER
    using VContainer;

    public class ObjectPoolInstall
    {
        public static void Install(IContainerBuilder builder) { builder.Register<ObjectPoolManager>(Lifetime.Singleton); }
    }

#else
    // Mono
#endif
}