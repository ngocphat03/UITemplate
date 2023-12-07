namespace UITemplate.Scripts.Interface
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IScreenPresenter
    {
        public string       ScreenId     { get; }
        public ScreenStatus ScreenStatus { get; }

        public void SetViewParent(Transform parent);

        public Transform GetViewParent();

        public Transform CurrentTransform { get; }

        public UniTask BindData();

        public UniTask OpenViewAsync();

        public UniTask CloseViewAsync();

        public void CloseView();

        public void HideView();

        public void DestroyView();
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