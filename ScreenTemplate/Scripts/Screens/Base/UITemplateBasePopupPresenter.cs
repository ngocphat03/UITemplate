namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base
{
    using AXitUnityTemplate.ScreenTemplate.Scripts.Interface;

    public abstract class UITemplateBasePopupPresenter<TView> : BasePopupPresenter<TView> where TView : IScreenView
    {
        protected override void OnViewReady() { base.OnViewReady(); }
    }

    public abstract class UITemplateBasePopupPresenter<TView, TModel> : BasePopupPresenter<TView, TModel> where TView : IScreenView
    {
        protected override void OnViewReady() { base.OnViewReady(); }
    }
}