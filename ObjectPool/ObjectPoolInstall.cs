namespace AXitUnityTemplate.ObjectPool
{
    using Zenject;

    public class ObjectPoolInstall : Installer<ObjectPoolInstall>
    {
        public override void InstallBindings() { this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy(); }
    }
}