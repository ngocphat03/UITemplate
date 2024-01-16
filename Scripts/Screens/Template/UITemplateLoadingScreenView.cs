namespace UITemplate.Scripts.Screens.Template
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UITemplate.Scripts.Screens.Base;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using Object = UnityEngine.Object;

    public class UITemplateLoadingScreenView : BaseView
    {
        [SerializeField] private Slider loadingSlider;
        private                  Tween  tween;
        private                  float  trueProgress;

        private void Start() { this.loadingSlider.value = 0f; }

        public void SetProgress(float progress)
        {
            if (!this.loadingSlider) return;
            if (progress <= this.trueProgress) return;
            this.tween.Kill();
            this.tween = DOTween.To(
                getter: () => this.loadingSlider.value,
                setter: value => this.loadingSlider.value = value,
                endValue: this.trueProgress = progress,
                duration: 0.5f
            );
        }

        public UniTask CompleteLoading()
        {
            this.SetProgress(1f);

            return this.tween.AsyncWaitForCompletion().AsUniTask();
        }
    }

    [ScreenInfo(nameof(UITemplateLoadingScreenView))]
    public class UITemplateLoadingScreenPresenter : UITemplateBaseScreenPresenter<UITemplateLoadingScreenView>
    {
        protected IGameAssets          GameAssets;
        private   float                loadingProgress;
        private   int                  loadingSteps;
        private   GameObject           objectPoolContainer;

        protected virtual string NextSceneName => "1.MainScene";

        private float LoadingProgress
        {
            get => this.loadingProgress;
            set
            {
                this.loadingProgress = value;
                this.View.SetProgress(value / this.loadingSteps);
            }
        }

        protected override void OnViewReady()
        {
            this.GameAssets = ObjectFactoryExtension.GetService<GameAssets>();
            base.OnViewReady();
            this.OpenViewAsync().Forget();
        }

        public override async UniTask BindData()
        {
            this.objectPoolContainer = new(nameof(this.objectPoolContainer));
            Object.DontDestroyOnLoad(this.objectPoolContainer);

            this.LoadingProgress = 0f;
            this.loadingSteps    = 1;

            await UniTask.WhenAll(
                this.Preload(),
                UniTask.WhenAll(
                    this.LoadUserData().ContinueWith(this.OnUserDataLoaded)
                ).ContinueWith(this.OnBlueprintAndUserDataLoaded)
            ).ContinueWith(this.OnLoadingCompleted).ContinueWith(this.LoadNextScene);

            await UniTask.CompletedTask;
        }

        protected virtual async UniTask LoadNextScene()
        {
            // await SceneManager.LoadSceneAsync(this.NextSceneName);
        }

        protected virtual AsyncOperationHandle<SceneInstance> LoadSceneAsync() { return this.GameAssets.LoadSceneAsync(this.NextSceneName, LoadSceneMode.Single, false); }

        private UniTask LoadUserData()
        {
            return UniTask.CompletedTask;
            // return this.TrackProgress(this.userDataManager.LoadUserData());
        }

        protected virtual UniTask OnBlueprintLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnUserDataLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnBlueprintAndUserDataLoaded() { return UniTask.CompletedTask; }

        protected virtual UniTask OnLoadingCompleted() { return UniTask.CompletedTask; }

        protected virtual UniTask Preload() { return UniTask.CompletedTask; }

        protected virtual UniTask PreloadAssets<T>(params object[] keys)
        {
            return UniTask.WhenAll(this.GameAssets.PreloadAsync<T>(this.NextSceneName, keys)
                .Select(this.TrackProgress));
        }

        protected virtual UniTask CreateObjectPool(string prefabName, int initialPoolSize = 1)
        {
            return this.TrackProgress(
                ObjectPoolManager.Instance.CreatePool(prefabName, initialPoolSize, this.objectPoolContainer));
            return UniTask.CompletedTask;
        }

        protected virtual UniTask TrackProgress(UniTask task)
        {
            ++this.loadingSteps;

            return task.ContinueWith(() => ++this.LoadingProgress);
        }

        protected virtual UniTask<T> TrackProgress<T>(AsyncOperationHandle<T> aoh)
        {
            ++this.loadingSteps;
            var localLoadingProgress = 0f;

            void UpdateProgress(float progress)
            {
                this.LoadingProgress += progress - localLoadingProgress;
                localLoadingProgress =  progress;
            }

            return aoh.ToUniTask(Progress.CreateOnlyValueChanged<float>(UpdateProgress))
                .ContinueWith(result =>
                {
                    UpdateProgress(1f);

                    return result;
                });
        }
    }
}