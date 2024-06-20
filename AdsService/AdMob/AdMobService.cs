#if ADMOB
namespace UITemplate.AdsService.AdMob
{
    using System;
    using Zenject;
    using System.Diagnostics;
    using GoogleMobileAds.Api;
    using UITemplate.AdsService.Scripts;
    using UITemplate.AdsService.Scripts.Enum;
    using UITemplate.AdsService.Scripts.Signals;
    using UITemplate.AdsService.Scripts.Interface;

    public class AdMobService : IAdService, IInitializable
    {
        public AdMobService(SignalBus signalBus, AdSetting adSetting)
        {
            this.signalBus = signalBus;
            this.adMobSettings = adSetting.AdNetworkBaseSetting as AdMobSettings;
        }

        private readonly SignalBus     signalBus;
        private readonly AdMobSettings adMobSettings;

        private BannerView     bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd     rewardedAd;

        public void Initialize()
        {
            UnityEngine.Debug.Log("You are using AdMob with: " +
                                 $"BannerId: {this.adMobSettings.BannerId}, " +
                                 $"InterstitialId: {this.adMobSettings.InterstitialId}, " +
                                 $"RewardedId: {this.adMobSettings.RewardedId}");
        }

        public void ShowBannerAd(BannerAdsPosition bannerAdsPosition = BannerAdsPosition.Bottom, int width = 320, int height = 50)
        {
            const string placement = "Banner";

            var size = new AdSize(width, height);
            var position = bannerAdsPosition.ToAdMobAdPosition();

            if (this.bannerView is not null)
            {
                this.bannerView.SetPosition(position);
                this.bannerView.Show();

                return;
            }

            this.bannerView = new BannerView(this.adMobSettings.BannerId, size, position);

            #region Events

            this.bannerView.OnBannerAdLoaded += () => this.signalBus.Fire(new BannerAdLoadedSignal(placement));
            this.bannerView.OnBannerAdLoadFailed += (error) => this.signalBus.Fire(new BannerAdLoadFailedSignal(placement, error.GetMessage()));
            this.bannerView.OnAdFullScreenContentOpened += () => this.signalBus.Fire(new BannerAdPresentedSignal(placement));
            this.bannerView.OnAdFullScreenContentClosed += () => this.signalBus.Fire(new BannerAdDismissedSignal(placement));
            this.bannerView.OnAdClicked += () => this.signalBus.Fire(new BannerAdClickedSignal(placement));

            #endregion

            this.bannerView.LoadAd(new AdRequest());
        }

        public void HideBannedAd() { this.bannerView?.Hide(); }

        public void DestroyBannerAd()
        {
            this.bannerView?.Destroy();
            this.bannerView = null;
        }

        public void LoadInterstitialAd(string place)
        {
            var stopwatch = Stopwatch.StartNew();
            InterstitialAd.Load(this.adMobSettings.InterstitialId, new AdRequest(), (ad, error) =>
            {
                stopwatch.Stop();
                if (error is not null)
                {
                    this.signalBus.Fire(new InterstitialAdLoadFailedSignal(place, error.GetMessage(), stopwatch.ElapsedMilliseconds));

                    return;
                }

                this.signalBus.Fire(new InterstitialAdLoadedSignal(place, stopwatch.ElapsedMilliseconds));
                this.interstitialAd?.Destroy();
                this.interstitialAd = ad;
            });
        }

        public void ShowInterstitialAd(string place)
        {
            #region Events

            this.interstitialAd.OnAdFullScreenContentOpened += () => this.signalBus.Fire(new InterstitialAdDisplayedSignal(place));
            this.interstitialAd.OnAdFullScreenContentClosed += () => this.signalBus.Fire(new InterstitialAdClosedSignal(place));
            this.interstitialAd.OnAdFullScreenContentFailed += _ => this.signalBus.Fire(new InterstitialAdDisplayedFailedSignal(place));
            this.interstitialAd.OnAdClicked += () => this.signalBus.Fire(new InterstitialAdClickedSignal(place));

            #endregion

            this.interstitialAd.Show();
        }

        public void LoadRewardAd(string place)
        {
            var stopwatch = Stopwatch.StartNew();
            RewardedAd.Load(this.adMobSettings.RewardedId, new AdRequest(), (ad, error) =>
            {
                stopwatch.Stop();
                if (error is not null)
                {
                    this.signalBus.Fire(new RewardedAdLoadFailedSignal(place, error.GetMessage(), stopwatch.ElapsedMilliseconds));

                    return;
                }

                this.signalBus.Fire(new RewardedAdLoadedSignal(place, stopwatch.ElapsedMilliseconds));
                this.rewardedAd?.Destroy();
                this.rewardedAd = ad;
            });
        }

        public void ShowRewardedAd(string place, Action onCompleted, Action onFailed)
        {
            #region Events

            this.rewardedAd.OnAdFullScreenContentOpened += () => this.signalBus.Fire(new RewardedAdDisplayedSignal(place));
            this.rewardedAd.OnAdFullScreenContentFailed += _ => OnAdFullScreenContentFailed();
            this.rewardedAd.OnAdClicked += () => this.signalBus.Fire(new RewardedAdClickedSignal(place));
            this.rewardedAd.OnAdPaid += _ => this.signalBus.Fire(new RewardedAdEligibleSignal(place));

            #endregion

            this.rewardedAd.Show(_ =>
            {
                this.signalBus.Fire(new RewardedAdCompletedSignal(place));
                this.signalBus.Fire(new RewardedAdClosedSignal(place));
                onCompleted?.Invoke();
            });
            this.signalBus.Fire(new RewardedAdCalledSignal(place));

            void OnAdFullScreenContentFailed()
            {
                this.signalBus.Fire(new RewardedSkippedSignal(place));
                onFailed?.Invoke();
                onFailed = null;
            }
        }
    }
}
#endif