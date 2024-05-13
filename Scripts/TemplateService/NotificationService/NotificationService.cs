namespace UITemplate.Scripts.TemplateService.NotificationService
{
    using Zenject;
    using UnityEngine;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;
    using UITemplate.Scripts.TemplateService.NotificationService.Signals;

    public class NotificationService : MonoBehaviour, IInitializable
    {
        [field: SerializeField] private NotificationMessenger NotificationMessenger { get; set; }
        [field: SerializeField] private Transform             StartPoint            { get; set; }
        [field: SerializeField] private Transform             EndPoint              { get; set; }

        private readonly List<NotificationMessenger> notificationMessengers = new();
        [Inject] public  SignalBus                   SignalBus;

        public void Initialize()
        {
            Object.DontDestroyOnLoad(this);
            this.SignalBus.Subscribe<NotificationSignal>(this.OnNotificationReceived);
        }
        
        public void ShowMessage(string message, float duration = 3)
        {
            var notificationMessenger = this.GetNotificationMessenger();
            notificationMessenger.ShowMessage(
                message: message,
                startPoint: this.StartPoint,
                endPoint: this.EndPoint,
                duration: duration);
        }

        private void OnNotificationReceived(NotificationSignal signal)
        {
            Debug.LogError("NotificationService: OnNotificationReceived");
            var notificationMessenger = this.GetNotificationMessenger();
            notificationMessenger.ShowMessage(
                message: signal.Message,
                startPoint: this.StartPoint,
                endPoint: this.EndPoint,
                duration: signal.Duration);
        }

        private NotificationMessenger GetNotificationMessenger()
        {
            foreach (var notificationMessenger in this.notificationMessengers)
            {
                if (!notificationMessenger.gameObject.activeSelf)
                {
                    notificationMessenger.gameObject.SetActive(true);
                    return notificationMessenger;
                }
            }

            var newNotificationMessenger = Object.Instantiate(this.NotificationMessenger, this.transform);
            this.notificationMessengers.Add(newNotificationMessenger);

            return newNotificationMessenger;
        }
    }
}