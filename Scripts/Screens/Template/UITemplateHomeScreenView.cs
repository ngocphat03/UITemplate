namespace UITemplate.Scripts.Screens.Template
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Screens.Base;
    using UnityEngine.UI;

    [ScreenPresenter(typeof(UITemplateHomeScreenPresenter))]
    public class UITemplateHomeScreenView : BaseView
    {
        public Button buttonPlay;
    }
    [ScreenInfo(nameof(UITemplateHomeScreenView))]
    public class UITemplateHomeScreenPresenter : BaseScreenPresenter<UITemplateHomeScreenView>
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
