namespace AXitUnityTemplate.GameAssets
{
    using Zenject;
    using AXitUnityTemplate.GameAssets.Interfaces;

    public class GameAssetInstall : Installer<GameAssetInstall>
    {
        public override void InstallBindings() { this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached(); }
    }
}