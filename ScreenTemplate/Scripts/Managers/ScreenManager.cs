namespace AXitUnityTemplate.ScreenTemplate.Scripts.Managers
{
    using System;
    using Zenject;
    using System.Linq;
    using UnityEngine;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;
    using AXitUnityTemplate.Utilities;
    using AXitUnityTemplate.GameAssets.Interfaces;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Interface;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Utilities;

    public interface IScreenManager
    {
        public UniTask<T> GetScreen<T>() where T : IScreenPresenter;

        public UniTask<T> OpenScreen<T>() where T : IScreenPresenter;

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>;

        public UniTask CloseCurrentScreen();
    }

    public class ScreenManager : MonoBehaviour, IScreenManager
    {
        [SerializeField] private Transform        screenOpen;
        [SerializeField] private Transform        screenClose;
        private                  IScreenPresenter activeScreens;

        private          IGameAssets                              gameAssets;
        private readonly Dictionary<Type, IScreenPresenter>       typeToLoadedScreenPresenter = new();
        private readonly Dictionary<Type, Task<IScreenPresenter>> typeToPendingScreen         = new();

        [Inject]
        public void Init(IGameAssets gameAsset)
        {
            this.gameAssets = gameAsset;
            this.ResearchScreen();
        }

        public void ResearchScreen()
        {
            if (this.screenOpen.childCount == 0) return;

            var listScreenDefault = this.screenOpen.GetComponentsInChildren<IScreenView>().ToList();
            listScreenDefault.ForEach(this.InitScreen);
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
                screenPresenter = this.GetCurrentContainer().Instantiate<T>();
                var screenInfo = screenPresenter.GetCustomAttribute<ScreenInfoAttribute>();
                var viewObject = Object.Instantiate(await this.gameAssets.LoadAssetAsync<GameObject>(screenInfo.AddressableScreenPath), this.screenOpen).GetComponent<IScreenView>();

                screenPresenter.SetView(viewObject, this.CloseScreen);
                this.typeToLoadedScreenPresenter.Add(screenType, screenPresenter);

                return (T)screenPresenter;
            }
        }

        public async UniTask<T> OpenScreen<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                if (nextScreen.GetCustomAttribute<PopupInfoAttribute>() is null || !nextScreen.GetCustomAttribute<PopupInfoAttribute>().IsOverlay)
                {
                    await this.CloseCurrentScreen();
                    this.activeScreens = nextScreen;
                }

                nextScreen.SetViewParent(this.screenOpen);
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }

            Debug.LogError($"The {typeof(T).Name} screen does not exist");

            return default;
        }

        public async UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>
        {
            var nextScreen = await this.GetScreen<TPresenter>();

            if (nextScreen != null)
            {
                nextScreen.SetViewParent(this.screenOpen);
                await nextScreen.OpenView(model);

                return nextScreen;
            }

            Debug.LogError($"The {typeof(TPresenter).Name} screen does not exist");

            return default;
        }

        public async UniTask<T> OpenPopup<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                nextScreen.SetViewParent(this.screenOpen);
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }

            Debug.LogError($"The {typeof(T).Name} screen does not exist");

            return default;
        }

        public async UniTask<TPresenter> OpenPopup<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>
        {
            var nextScreen = (await this.GetScreen<TPresenter>());

            if (nextScreen != null)
            {
                nextScreen.SetViewParent(this.screenOpen);
                await nextScreen.OpenView(model);

                return nextScreen;
            }

            Debug.LogError($"The {typeof(TPresenter).Name} screen does not exist");

            return default;
        }

        public async UniTask CloseCurrentScreen()
        {
            if (this.activeScreens is null) return;

            await this.activeScreens.CloseViewAsync();
        }

        private void CloseScreen(IScreenPresenter screenPresenter) { screenPresenter.SetViewParent(this.screenClose); }

        private async void InitScreen(IScreenView screenView)
        {
            var screenPresenter = screenView.GetCustomAttribute<ScreenPresenterAttribute>().ScreenPresenter();
            var typePresenter   = screenPresenter.GetType();
            screenPresenter.SetView(screenView, this.CloseScreen);
            this.typeToLoadedScreenPresenter.Add(typePresenter, screenPresenter);
            await screenPresenter.OpenViewAsync();
        }

        public IScreenPresenter GetCurrentScreen() { return this.activeScreens; }
    }
}