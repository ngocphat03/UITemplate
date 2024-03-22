namespace UITemplate.Scripts.Screens.Base
{
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Interface;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class BaseView : MonoBehaviour, IScreenView
    {
        [SerializeField] private CanvasGroup   viewRoot;
        public                   RectTransform RectTransform { get;                  private set; }
        public                   bool          IsReadyToUse  { get;                  private set; }
        protected virtual        CanvasGroup   ViewRoot      { get => this.viewRoot; set => this.viewRoot = value; }
        public                   bool          blocksRaycasts = true;

        private void Awake()
        {
            this.viewRoot = this.GetComponent<CanvasGroup>();
            if (this.viewRoot is null)
            {
            }

            if (this.RectTransform is null)
            {
                this.RectTransform = this.GetComponent<RectTransform>();
            }

            this.UpdateAlpha(0);
            this.IsReadyToUse = true;
        }

        public virtual UniTask Open()
        {
            this.UpdateAlpha(1f);

            return UniTask.CompletedTask;
        }

        public UniTask Close()
        {
            this.UpdateAlpha(0);

            return UniTask.CompletedTask;
        }

        public void Hide() { this.UpdateAlpha(0); }

        public void Show() { this.UpdateAlpha(1); }

        public void DestroySelf() { Destroy(this.gameObject); }

        private void UpdateAlpha(float value)
        {
            if (this.viewRoot == null) return;
            this.ViewRoot.alpha          = value;
            this.ViewRoot.blocksRaycasts = this.blocksRaycasts && value >= 1;
        }
    }
}