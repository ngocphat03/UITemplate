#if ADS_SERVICE
namespace UITemplate.AdsService.Scripts.Interface
{
    using System;
    using UITemplate.AdsService.Scripts.Enum;

    public interface IAdService
    {
        #region Banner

        public void ShowBannerAd(BannerAdsPosition bannerAdsPosition = BannerAdsPosition.Bottom, int width = 320, int height = 50);

        public void HideBannedAd();

        public void DestroyBannerAd();

        #endregion

        #region Interstitial

        public void LoadInterstitialAd(string place);

        public void ShowInterstitialAd(string place);

        #endregion

        #region Reward

        public void LoadRewardAd(string place);

        public void ShowRewardedAd(string place, Action onCompleted, Action onFailed);

        #endregion
    }
}

#endif