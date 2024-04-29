namespace UITemplate.Scripts.Screens.Template
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.Sound;
    using UITemplate.Scripts.Screens.Base;
    using UnityEngine.UI;

    public class UITemplateSettingPopupView : BaseView
    {
        public Slider sliderMusic;
        public Slider sliderSound;
        public Button buttonClose;
    }

    [PopupInfo(nameof(UITemplateSettingPopupView), isOverlay: true)]
    public class UITemplateSettingPopupPresenter : BasePopupPresenter<UITemplateSettingPopupView>
    {
        private readonly SoundManager soundManager;

        public UITemplateSettingPopupPresenter(SoundManager soundManager) { this.soundManager = soundManager; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.sliderMusic.onValueChanged.AddListener(this.OnMusicValueChanged);
            this.View.sliderSound.onValueChanged.AddListener(this.OnSoundValueChanged);
            this.View.buttonClose.onClick.AddListener(this.OnClose);
        }

        public override UniTask BindData() { return UniTask.CompletedTask; }

        public async void OnClose() { await this.CloseViewAsync(); }

        public void OnMusicValueChanged(float value) { this.soundManager.ChangeVolumeMusic(value); }

        public void OnSoundValueChanged(float value) { this.soundManager.ChangeVolumeSound(value); }
    }
}