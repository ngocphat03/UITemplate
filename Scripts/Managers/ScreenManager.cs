namespace UITemplate.Scripts.Managers
{
    using System;
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
        private IGameAssets gameAssets = new GameAssets();

        private Dictionary<Type, IScreenPresenter>       typeToLoadedScreenPresenter = new Dictionary<Type, IScreenPresenter>();
        private Dictionary<Type, Task<IScreenPresenter>> typeToPendingScreen         = new Dictionary<Type, Task<IScreenPresenter>>();

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
                var viewObject = Instantiate(await this.gameAssets.LoadAssetAsync<GameObject>(screenType.Name));
                screenPresenter = viewObject.GetComponent<T>();

                return (T)screenPresenter;
            }
        }

        public async UniTask<T> OpenScreen<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }
            else
            {
                Debug.LogError($"The {typeof(T).Name} screen does not exist");

                return default;
            }
        }

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel> { throw new System.NotImplementedException(); }

        public UniTask CloseCurrentScreen() { throw new System.NotImplementedException(); }
    }
}