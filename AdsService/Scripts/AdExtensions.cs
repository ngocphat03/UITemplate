#if ADS_SERVICE
namespace UITemplate.AdsService.Scripts
{
    using GoogleMobileAds.Api;
    using UITemplate.AdsService.Scripts.Enum;

    public static class AdExtensions
    {
#if ADMOB
        public static AdPosition ToAdMobAdPosition(this BannerAdsPosition bannerAdsPosition)
        {
            return bannerAdsPosition switch
            {
                BannerAdsPosition.Top         => AdPosition.Top,
                BannerAdsPosition.Bottom      => AdPosition.Bottom,
                BannerAdsPosition.TopLeft     => AdPosition.TopLeft,
                BannerAdsPosition.TopRight    => AdPosition.TopRight,
                BannerAdsPosition.BottomLeft  => AdPosition.BottomLeft,
                BannerAdsPosition.BottomRight => AdPosition.BottomRight,
                _                             => AdPosition.Bottom,
            };
        }
#endif
    }
}
#endif