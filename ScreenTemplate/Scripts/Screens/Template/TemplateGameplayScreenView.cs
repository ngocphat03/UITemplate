namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Template
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Utilities;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base;

    [ScreenPresenter(typeof(TemplateGameplayScreenPresenter))]
    public class TemplateGameplayScreenView : BaseView
    {
    }

    [ScreenInfo(nameof(TemplateGameplayScreenView))]
    public class TemplateGameplayScreenPresenter : BaseScreenPresenter<TemplateGameplayScreenView>
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