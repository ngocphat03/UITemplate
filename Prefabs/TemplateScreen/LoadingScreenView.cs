namespace UITemplate.Prefabs.TemplateScreen
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Screens.Template;
    using UnityEngine;

    [ScreenPresenterAttribute(typeof(LoadingScreenPresenter))]
    public class LoadingScreenView : UITemplateLoadingScreenView
    {
    }

    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter : UITemplateLoadingScreenPresenter
    {
        protected override void OnViewReady()
        {
            Debug.Log("onvsdvsv");
        }

        public override UniTask BindData()
        {
            Debug.Log("fdsjksdkfsdkhfsdjd");
            return UniTask.CompletedTask;
        }
    }
}