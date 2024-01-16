namespace UITemplate.Scripts.Screens.Template
{
    using Cysharp.Threading.Tasks;
    using global::UITemplate.Scripts.Extension;
    using global::UITemplate.Scripts.Screens.Base;
    using UnityEngine;

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