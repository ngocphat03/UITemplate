namespace UITemplate.Scripts.Screens.Template
{
    using System;
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Screens.Base;
    using UnityEngine;

    public class UITemplateSettingPopupView : BaseView
    {
        public Action hehe;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.hehe();
            }
        }
    }

    [PopupInfo(nameof(UITemplateSettingPopupView), isOverlay: true)]
    public class UITemplateSettingPopupPresenter : BasePopupPresenter<UITemplateSettingPopupView>
    {
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.hehe = this.CloseView;
        }

        public override async UniTask BindData()
        {
        }
    }
}