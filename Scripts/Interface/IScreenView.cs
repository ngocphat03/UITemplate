namespace UITemplate.Scripts.Interface
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IScreenView
    {
        public RectTransform RectTransform { get; }
        public bool          IsReadyToUse  { get; }

        public UniTask Open();

        public UniTask Close();

        public void Hide();

        public void Show();

        public void DestroySelf();
    }
}