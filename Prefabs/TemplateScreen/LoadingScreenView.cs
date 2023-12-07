namespace UITemplate.Prefabs.TemplateScreen
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Screens.Base;

    public class LoadingScreenView : BaseView
    {
    }

    public class LoadingScreenPresenter : BaseScreenPresenter<LoadingScreenView>
    {
        protected override void OnViewReady() { base.OnViewReady(); }

        public override    UniTask BindData() { return UniTask.CompletedTask; }
    }
}