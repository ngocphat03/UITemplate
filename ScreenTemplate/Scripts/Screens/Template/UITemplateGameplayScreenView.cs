namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Template
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base;

    [ScreenPresenter(typeof(UITemplateGameplayScreenPresenter))]
    public class UITemplateGameplayScreenView : BaseView
    {
    }

    [ScreenInfo(nameof(UITemplateGameplayScreenView))]
    public class UITemplateGameplayScreenPresenter : BaseScreenPresenter<UITemplateGameplayScreenView>
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