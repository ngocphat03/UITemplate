namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Template
{
    using UnityEngine.UI;
    using Cysharp.Threading.Tasks;
    using AXitUnityTemplate.Sound;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Utilities;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base;

    public class TemplateSettingPopupView : BaseView
    {
        public Slider sliderMusic;
        public Slider sliderSound;
        public Button buttonClose;
    }

    [PopupInfo(nameof(TemplateSettingPopupView), isOverlay: true)]
    public class TemplateSettingPopupPresenter : BasePopupPresenter<TemplateSettingPopupView>
    {
        private readonly SoundManager soundManager;

        public TemplateSettingPopupPresenter(SoundManager soundManager) { this.soundManager = soundManager; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.sliderMusic.onValueChanged.AddListener(this.OnMusicValueChanged);
            this.View.sliderSound.onValueChanged.AddListener(this.OnSoundValueChanged);
            this.View.buttonClose.onClick.AddListener(this.OnClose);
        }

        public override UniTask BindData() { return UniTask.CompletedTask; }

        protected virtual void OnClose() { this.CloseViewAsync().Forget(); }

        protected virtual void OnMusicValueChanged(float value) { this.soundManager.ChangeVolumeMusic(value); }

        protected virtual void OnSoundValueChanged(float value) { this.soundManager.ChangeVolumeSound(value); }
    }
}