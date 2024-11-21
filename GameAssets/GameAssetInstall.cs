namespace AXitUnityTemplate.GameAssets
{
    using AXitUnityTemplate.GameAssets.Interfaces;

#if ZENJECT
    using Zenject;

    public class GameAssetInstall : Installer<GameAssetInstall>
    {
        public override void InstallBindings() { this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached(); }
    }
#elif VCONTAINER
    using VContainer;

    public class GameAssetInstall
    {
        public static void Install(IContainerBuilder builder) { builder.Register<IGameAssets, GameAssets>(Lifetime.Singleton); }
    }
#else
    using UnityEngine;

    public class GameAssetSingleton : MonoBehaviour
    {
        [field: SerializeField] private bool donDestroyOnLoad;

        public static IGameAssets Instance { get; private set; }

        private void Awake()
        {
            if (this.donDestroyOnLoad) UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            GameAssetSingleton.Instance = new GameAssets();
        }
    }
#endif
}