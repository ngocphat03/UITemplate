namespace UITemplate.Prefabs.TemplateScreen
{
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Screens.Template;

    [ScreenPresenterAttribute(typeof(LoadingScreenPresenter))]
    public class LoadingScreenView : UITemplateLoadingScreenView
    {
    }

    [ScreenInfo(nameof(LoadingScreenView))]
    public class LoadingScreenPresenter : UITemplateLoadingScreenPresenter
    {
    }
}