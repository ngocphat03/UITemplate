namespace UITemplate.Prefabs.TemplateScreen
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Screens.Base;
    using UnityEngine;

    public class LoadingScreenView : BaseView
    {
    }

    public class LoadingScreenPresenter : BaseScreenPresenter<LoadingScreenView>
    {
        protected override void OnViewReady()
        {
            Debug.Log("On view ready");
            base.OnViewReady();
        }

        public override UniTask BindData()
        {
            Debug.Log("Bind data");
            return UniTask.CompletedTask;
        }
    }
}