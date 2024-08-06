namespace AXitUnityTemplate.ScreenTemplate.Scripts.Screens.Base
{
    using System;
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using AXitUnityTemplate.ScreenTemplate.Scripts.Interface;
    using AXitUnityTemplate.ScreenTemplate.Scripts.ScreenTransition;

    public class BaseView : MonoBehaviour, IScreenView
    {
        [SerializeField] private CanvasGroup      viewRoot;
        [SerializeField] private ScreenTransition screenTransition;

        #region Public Properties

        public RectTransform RectTransform { get; private set; }
        public bool          IsReadyToUse  { get; private set; }
        public bool          blockRaycastHit = true;
        public event Action  OnOpen;
        public event Action  OnClose;
        public event Action  OnDestroy;

        #endregion

        protected virtual CanvasGroup      ViewRoot         { get => this.viewRoot;         set => this.viewRoot = value; }
        protected virtual ScreenTransition ScreenTransition { get => this.screenTransition; set => this.screenTransition = value; }

        private void Awake()
        {
            this.viewRoot         = this.GetComponent<CanvasGroup>();
            this.screenTransition = this.transform.GetComponent<ScreenTransition>();
            this.RectTransform    = this.GetComponent<RectTransform>();

            this.UpdateAlpha(0);
            this.IsReadyToUse = true;
        }

        public virtual async UniTask Open()
        {
            this.UpdateAlpha(1f);
            await this.screenTransition.PlayIntroAnimation();
            this.OnOpen?.Invoke();
        }

        public virtual async UniTask Close()
        {
            await this.screenTransition.PlayOutroAnimation();
            this.UpdateAlpha(0);
            this.OnClose?.Invoke();
        }

        public void Hide() { this.UpdateAlpha(0); }

        public void Show() { this.UpdateAlpha(1); }

        public void DestroySelf()
        {
            this.OnDestroy?.Invoke();
            UnityEngine.Object.Destroy(this.gameObject);
        }

        private void UpdateAlpha(float value)
        {
            if (this.viewRoot == null) return;
            this.ViewRoot.alpha          = value;
            this.ViewRoot.blocksRaycasts = this.blockRaycastHit && value >= 1;
        }
    }
}