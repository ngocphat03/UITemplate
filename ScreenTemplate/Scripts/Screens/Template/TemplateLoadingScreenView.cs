namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Template
{
    using AXitUnityTemplate.Blueprint.BlueprintControlFlow;
    using AXitUnityTemplate.Blueprint.Signals;
    using TMPro;
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;
    using Cysharp.Threading.Tasks;
    using Object = UnityEngine.Object;
    using UnityEngine.SceneManagement;
    using AXitUnityTemplate.ObjectPool;
    using AXitUnityTemplate.GameAssets.Interfaces;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Utilities;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base;
    using AXitUnityTemplate.UserData.UserDataManager;
    using Zenject;

    [ScreenPresenter(typeof(TemplateLoadingScreenPresenter))]
    public class TemplateLoadingScreenView : BaseView
    {
        [SerializeField] private Slider          loadingSlider;
        [SerializeField] private TextMeshProUGUI loadingProgressTxt;

        private  Tween  tween;
        private  float  trueProgress;
        internal string LoadingText;

        private void Start() { this.loadingSlider.value = 0f; }

        public void SetProgress(float progress)
        {
            if (!this.loadingSlider) return;
            if (progress <= this.trueProgress) return;
            this.tween.Kill();
            this.tween = DOTween.To(
                getter: () => this.loadingSlider.value,
                setter: value =>
                {
                    this.loadingSlider.value = value;
                    if (this.loadingProgressTxt != null)
                        this.loadingProgressTxt.text = string.Format(this.LoadingText, (int)(value * 100));
                },
                endValue: this.trueProgress = progress,
                duration: 0.5f
            ).SetUpdate(true);
        }

        public UniTask CompleteLoading()
        {
            this.SetProgress(1f);

            return this.tween.AsyncWaitForCompletion().AsUniTask();
        }
    }

    [ScreenInfo(nameof(TemplateLoadingScreenView))]
    public class TemplateLoadingScreenPresenter : BaseScreenPresenter<TemplateLoadingScreenView>
    {
        protected readonly UserDataManager userDataManager;
        protected readonly UserLocalData userLocalData;
        public TemplateLoadingScreenPresenter(IGameAssets            gameAssets,
                                              SignalBus              signalBus,
                                              BlueprintReaderManager blueprintReaderManager,
                                              UserDataManager userDataManager,
                                              UserLocalData userLocalData)
        {
            this.GameAssets             = gameAssets;
            this.signalBus              = signalBus;
            this.blueprintReaderManager = blueprintReaderManager;
            this.userDataManager        = userDataManager;
            this.userLocalData          = userLocalData;
        }

        protected virtual  string                 NextSceneName => "1.MainScene";
        protected readonly IGameAssets            GameAssets;
        private readonly   SignalBus              signalBus;
        private readonly   BlueprintReaderManager blueprintReaderManager;

        private float      loadingProgress;
        private int        loadingSteps = 1;
        private GameObject objectPoolContainer;

        private float LoadingProgress
        {
            get => this.loadingProgress;
            set
            {
                this.loadingProgress = value;
                this.View.SetProgress(value / this.loadingSteps);
            }
        }

        protected override async void OnViewReady()
        {
            base.OnViewReady();

            this.objectPoolContainer = new(nameof(this.objectPoolContainer));
            Object.DontDestroyOnLoad(this.objectPoolContainer);

            this.LoadingProgress = 0f;

            await UniTask.WhenAll(
                this.Preload(),
                UniTask.WhenAll(
                    this.LoadBlueprint().ContinueWith(this.OnBlueprintLoaded),
                    this.LoadUserData().ContinueWith(this.OnUserDataLoaded)
                ).ContinueWith(this.OnBlueprintAndUserDataLoaded)
            ).ContinueWith(this.OnLoadingCompleted).ContinueWith(this.LoadNextScene);
        }

        public override async UniTask BindData() { await UniTask.CompletedTask; }

        protected virtual async UniTask LoadNextScene()
        {
            var nextScene = await this.TrackProgress(this.LoadSceneAsync());
            await this.View.CompleteLoading();
            await nextScene.ActivateAsync();
        }

        protected virtual AsyncOperationHandle<SceneInstance> LoadSceneAsync() { return this.GameAssets.LoadSceneAsync(this.NextSceneName, LoadSceneMode.Single, false); }

        private UniTask LoadBlueprint()
        {
            this.TrackProgress<LoadBlueprintDataProgressSignal>();
            this.TrackProgress<ReadBlueprintProgressSignal>();

            return this.blueprintReaderManager.LoadBlueprint();
        }

        private UniTask LoadUserData() { return this.TrackProgress(this.userDataManager.LoadUserData());  }

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
        }protected void TrackProgress<T>() where T : IProgressPercent
        {
            ++this.loadingSteps;
            var localLoadingProgress = 0f;

            this.signalBus.Subscribe<T>(UpdateProgress);

            void UpdateProgress(T progress)
            {
                this.LoadingProgress += progress.Percent - localLoadingProgress;
                localLoadingProgress =  progress.Percent;
                if (progress.Percent >= 1f)
                {
                    this.signalBus.Unsubscribe<T>(UpdateProgress);
                }
            }
        }
    }
}