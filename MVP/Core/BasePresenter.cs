namespace AXitUnityTemplate.MVP.Core
{
    using Cysharp.Threading.Tasks;
    using AXitUnityTemplate.MVP.Interface;

    public abstract class BasePresenter<TModel, TView> : IPresenter
    where TModel : IModel
    where TView : IView
    {
        public TModel Model { get; set; }
        public TView  View  { get; set; }

        public virtual void Initialize(IModel model) { }

        public abstract UniTask CreateViewAsync();

        public abstract UniTask DestroyViewAsync();
    }
}