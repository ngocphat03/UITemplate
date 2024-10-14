namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Template
{
    using UnityEngine.UI;
    using Cysharp.Threading.Tasks;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Utilities;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base;

    [ScreenPresenter(typeof(TemplateHomeScreenPresenter))]
    public class TemplateHomeScreenView : BaseView
    {
        public Button buttonPlay;
    }
    [ScreenInfo(nameof(TemplateHomeScreenView))]
    public class TemplateHomeScreenPresenter : BaseScreenPresenter<TemplateHomeScreenView>
    {
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.buttonPlay.onClick.AddListener(this.OnPlay);
        }
        
        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }
        
        protected virtual void OnPlay() {  }
    }
}
