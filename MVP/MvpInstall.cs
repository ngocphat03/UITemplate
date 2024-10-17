namespace AXitUnityTemplate.MVP
{
    using Zenject;
    using AXitUnityTemplate.MVP.Core;

    public class MvpInstall : Installer<MvpInstall>
    {
        public override void InstallBindings() { this.Container.Bind<MvpFactory>().AsSingle().NonLazy(); }
    }
}