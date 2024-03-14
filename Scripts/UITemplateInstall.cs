namespace UITemplate.Scripts
{
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.ObjectPool;
    using Zenject;

    public class UITemplateInstall : Installer<UITemplateInstall>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(this.Container);
            this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached();
            this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy();
        }
    }
}