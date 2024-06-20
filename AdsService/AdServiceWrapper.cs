#if ADS_SERVICE
namespace UITemplate.AdsService
{
    using System;
    using Zenject;
    using UITemplate.AdsService.Scripts.Interface;
    using UITemplate.AdsService.Scripts.Signals;

    public class AdServiceWrapper : IInitializable, IDisposable
    {
        public AdServiceWrapper(SignalBus signalBus, IAdService adService)
        {
            this.signalBus = signalBus;
            this.adService = adService;
        }
        private readonly SignalBus  signalBus;
        private readonly IAdService adService;

        public void Initialize() { }

        public virtual void ShowBannerAd(int width = 320, int height = 50) { this.adService.ShowBannerAd(width: width, height: height); }

        public virtual void ShowInterstitialAd(string place, Action closeInterstitialAd = null)
        {
            this.signalBus.Subscribe<InterstitialAdClosedSignal>(OnCloseInterstitialAd);
            this.adService.ShowInterstitialAd(place);

            void OnCloseInterstitialAd()
            {
                this.signalBus.Unsubscribe<InterstitialAdClosedSignal>(OnCloseInterstitialAd);
                closeInterstitialAd?.Invoke();
            }
        }

        public virtual void ShowRewardedAd(string place, Action onComplete, Action onFail = null) { this.adService.ShowRewardedAd(place, onComplete, onFail); }

        public void Dispose() { }
    }
}
#endif