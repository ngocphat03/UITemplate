namespace UITemplate.Scripts.Managers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Interface;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IScreenManager
    {
        public UniTask<T> GetScreen<T>() where T : IScreenPresenter;

        public UniTask<T> OpenScreen<T>() where T : IScreenPresenter;

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>;

        public UniTask CloseCurrentScreen();
    }

    public class ScreenManager : Singleton<ScreenManager>, IScreenManager
    {
        [SerializeField] private Transform        screenOpen;
        [SerializeField] private Transform        screenClose;
        private                  IScreenPresenter activeScreens;

        private readonly IGameAssets                              gameAssets                  = new GameAssets();
        private readonly Dictionary<Type, IScreenPresenter>       typeToLoadedScreenPresenter = new Dictionary<Type, IScreenPresenter>();
        private readonly Dictionary<Type, Task<IScreenPresenter>> typeToPendingScreen         = new Dictionary<Type, Task<IScreenPresenter>>();

        protected override void OnAwake()
        {
            base.OnAwake();
            
        }

        public async UniTask<T> GetScreen<T>() where T : IScreenPresenter
        {
            var screenType = typeof(T);

            if (this.typeToLoadedScreenPresenter.TryGetValue(screenType, out var screenPresenter)) return (T)screenPresenter;

            if (!this.typeToPendingScreen.TryGetValue(screenType, out var loadingTask))
            {
                loadingTask = InstantiateScreen();
                this.typeToPendingScreen.Add(screenType, loadingTask);
            }

            var result = await loadingTask;
            this.typeToPendingScreen.Remove(screenType);

            return (T)result;

            async Task<IScreenPresenter> InstantiateScreen()
            {
                screenPresenter = Activator.CreateInstance<T>();
                var screenInfo = screenPresenter.GetCustomAttribute<ScreenInfoAttribute>();
                var viewObject = Instantiate(await this.gameAssets.LoadAssetAsync<GameObject>(screenInfo.AddressableScreenPath), this.screenOpen).GetComponent<IScreenView>();
                screenPresenter.SetView(viewObject);
                this.typeToLoadedScreenPresenter.Add(screenType, screenPresenter);

                return (T)screenPresenter;
            }
        }

        public async UniTask<T> OpenScreen<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                await this.CloseCurrentScreen();
                nextScreen.SetViewParent(this.screenOpen);
                this.activeScreens = nextScreen;
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }
            else
            {
                Debug.LogError($"The {typeof(T).Name} screen does not exist");

                return default;
            }
        }

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>
        {
            Debug.LogError("Don't define this case :v");

            return new UniTask<TPresenter>();
        }

        public async UniTask CloseCurrentScreen()
        {
            if (this.activeScreens == null) return;
            this.activeScreens.SetViewParent(this.screenClose);
            await this.activeScreens.CloseViewAsync();
        }
    }
}