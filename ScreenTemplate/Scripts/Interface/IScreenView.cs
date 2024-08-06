namespace AXitUnityTemplate.ScreenTemplate.Scripts.Interface
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;

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