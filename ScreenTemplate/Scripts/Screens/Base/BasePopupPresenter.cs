namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base
{
    using System;
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.SceneManagement;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Interface;

    public abstract class BasePopupPresenter<TView> : IScreenPresenter where TView : IScreenView
    {
        public string                   ScreenId     { get; private set; }
        public ScreenStatus             ScreenStatus { get; protected set; } = ScreenStatus.Closed;
        public Action<IScreenPresenter> OnCloseView  { get; set; }

        public TView View;

        public async void SetView(IScreenView viewInstance, Action<IScreenPresenter> onClose = null)
        {
            this.View        = (TView)viewInstance;
            this.ScreenId    = $"{SceneManager.GetActiveScene().name}/{typeof(TView).Name}";
            this.OnCloseView = onClose;
            if (this.View.IsReadyToUse)
            {
                this.OnViewReady();
            }
            else
            {
                await UniTask.WaitUntil(() => this.View.IsReadyToUse);
                this.OnViewReady();         
            }
        }

        protected virtual void OnViewReady() { }

        public void SetViewParent(Transform parent)
        {
            if (parent == null)
            {
                Debug.LogWarning($"{parent.name} is null");

                return;
            }

            if (this.View.Equals(null)) return;
            this.View.RectTransform.SetParent(parent);
        }

        public Transform GetViewParent() => this.View.RectTransform.parent;

        public Transform CurrentTransform => this.View.RectTransform;

        public abstract UniTask BindData();

        public virtual async UniTask OpenViewAsync()
        {
            await this.BindData();

            if (this.ScreenStatus == ScreenStatus.Opened) return;
            this.ScreenStatus = ScreenStatus.Opened;
            await this.View.Open();
        }

        public virtual async UniTask CloseViewAsync()
        {
            if (this.ScreenStatus == ScreenStatus.Closed) return;
            this.ScreenStatus = ScreenStatus.Closed;
            await this.View.Close();
            this.OnCloseView?.Invoke(this);
            this.Dispose();
        }

        public virtual void Dispose() { }

        public virtual async void CloseView() { await this.CloseViewAsync(); }

        public virtual void HideView()
        {
            if (this.ScreenStatus == ScreenStatus.Hide) return;
            this.ScreenStatus = ScreenStatus.Hide;
            this.View.Hide();
            this.Dispose();
        }

        public void DestroyView()
        {
            if (this.ScreenStatus == ScreenStatus.Destroyed) return;
            this.ScreenStatus = ScreenStatus.Destroyed;

            if (this.View.Equals(null)) return;
            this.Dispose();
            this.View.DestroySelf();
        }

    }

    public abstract class BasePopupPresenter<TView, TModel> : BasePopupPresenter<TView>, IScreenPresenter where TView : IScreenView
    {
        protected TModel Model;

        public override async UniTask OpenViewAsync()
        {
            if (this.Model != null)
            {
                await this.BindData(this.Model);
            }
            else
            {
                Debug.LogWarning($"{this.GetType().Name} don't have Model!!!");
            }

            await base.OpenViewAsync();
        }

        public virtual async UniTask OpenView(TModel model)
        {
            this.Model = model != null ? model : this.Model;
            await this.OpenViewAsync();
        }

        public sealed override UniTask BindData() { return UniTask.CompletedTask; }

        public abstract UniTask BindData(TModel screenModel);
    }
}