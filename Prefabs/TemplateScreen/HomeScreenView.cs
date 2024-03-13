
namespace UITemplate.Prefabs.TemplateScreen
{
    using UITemplate.Scripts.Screens.Base;
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UnityEngine;

    [ScreenPresenter(typeof(HomeScreenPresenter))]
    public class HomeScreenView : BaseView
    {
        
    }
    [ScreenInfo(nameof(HomeScreenView))]
    public class HomeScreenPresenter : BaseScreenPresenter<HomeScreenView>
    {
        protected override void OnViewReady()
        {
            base.OnViewReady();
        }

        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }
    }
}
