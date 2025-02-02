namespace AXitUnityTemplate.ScreenTemplate.Scripts.Interface
{
    using System;
    using UnityEngine;
    using Cysharp.Threading.Tasks;

    public interface IScreenPresenter
    {
        public string                   ScreenId     { get; }
        public ScreenStatus             ScreenStatus { get; }
        public Action<IScreenPresenter> OnCloseView  { get; set; }

        public void SetViewParent(Transform parent);

        public Transform GetViewParent();

        public Transform CurrentTransform { get; }

        public UniTask BindData();

        public UniTask OpenViewAsync();

        public UniTask CloseViewAsync();

        public void CloseView();

        public void HideView();

        public void DestroyView();

        public void SetView(IScreenView viewInstance, Action<IScreenPresenter> onClose = null);
    }

    public interface IScreenPresenter<in TModel> : IScreenPresenter
    {
        public UniTask OpenView(TModel model);
    }

    public enum ScreenStatus
    {
        Opened,
        Closed,
        Hide,
        Destroyed,
    }
}