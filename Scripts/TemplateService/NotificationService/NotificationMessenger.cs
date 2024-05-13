namespace UITemplate.Scripts.TemplateService.NotificationService
{
    using TMPro;
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;
    using Cysharp.Threading.Tasks;

    public class NotificationMessenger : MonoBehaviour
    {
        [field: SerializeField] private TMP_Text NotificationText { get; set; }
        [field: SerializeField] private Button   ButtonClose      { get; set; }

        private Vector3 startPosition, endPosition;
        private Tween   tweenMoving;
        private bool    usingMessage;

        private const float Speed = 0.25f;

        private void Awake() { this.ButtonClose.onClick.AddListener(this.HideMessage); }

        public async void ShowMessage(string message, Transform startPoint, Transform endPoint, float duration = 2f)
        {
            // Set default position
            this.startPosition = startPoint.position;
            this.endPosition   = endPoint.position;

            // Init data
            this.NotificationText.text = message;
            this.usingMessage         = true;

            // Animation
            this.transform.position = this.startPosition;
            this.tweenMoving        = this.transform.DOMoveY(this.endPosition.y, NotificationMessenger.Speed).SetEase(Ease.InOutSine);

            // Auto hide message after duration
            await UniTask.Delay(TimeSpan.FromSeconds(duration)).ContinueWith(this.HideMessage);
        }

        public void HideMessage()
        {
            if(!this.usingMessage) return;
            
            this.usingMessage = false;
            this.tweenMoving?.Kill();
            this.transform.DOMoveY(this.startPosition.y, NotificationMessenger.Speed)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => this.gameObject.SetActive(false));
        }
    }
}