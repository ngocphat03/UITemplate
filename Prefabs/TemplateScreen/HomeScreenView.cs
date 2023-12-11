
namespace UITemplate.Prefabs.TemplateScreen
{
    using UITemplate.Scripts.Screens.Base;
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UnityEngine;

    public class HomeScreenView : BaseView
    {
        
    }
    [ScreenInfo(nameof(HomeScreenView))]
    public class HomeScreenPresenter : BaseScreenPresenter<HomeScreenView>
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
