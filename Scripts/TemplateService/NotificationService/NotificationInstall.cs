namespace UITemplate.Scripts.TemplateService.NotificationService
{
    using Zenject;
    using UnityEngine;
    using UITemplate.Scripts.TemplateService.NotificationService.Signals;

    public class NotificationInstall : Installer<NotificationInstall>
    {
        public override void InstallBindings()
        {
            this.Container.DeclareSignal<NotificationSignal>();

            this.Container.BindInterfacesAndSelfTo<NotificationService>().FromInstance(Object.FindObjectOfType<NotificationService>()).AsSingle().NonLazy();
        }
    }
}