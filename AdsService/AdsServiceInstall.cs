#if ADS_SERVICE
namespace UITemplate.AdsService
{
    using System;
    using Zenject;
    using UnityEngine;
#if ADMOB
    using UITemplate.AdsService.AdMob;
#endif
    using UITemplate.AdsService.Scripts.Base;
    using UITemplate.AdsService.Scripts.Signals;
    using UITemplate.AdsService.Scripts.Interface;

    public class AdsServiceInstall : Installer<AdsServiceInstall>
    {
        private string               adsSettingPath;
        private AdNetworkBaseSetting adNetworkSetting;

        public override void InstallBindings()
        {
            this.InstallSignals();
            this.LoadNetworkSetting();
            this.BindNetWorkSetting();
            this.BindAdService();
            this.Container.BindInterfacesAndSelfTo<AdServiceWrapper>().AsCached().NonLazy();
        }

        private void LoadNetworkSetting()
        {
#if ADMOB
            this.adsSettingPath   = "AdMobSettings";
            this.adNetworkSetting = Resources.Load<AdMobSettings>(this.adsSettingPath);
#endif
            if (this.adsSettingPath is null) throw new Exception($"Please add symbol for ad network. Example: ADMOB");
        }

        private void BindNetWorkSetting()
        {
#if ADMOB
            this.Container.BindInstance(this.adNetworkSetting).AsSingle();
            this.Container.Bind<AdSetting>().AsSingle().NonLazy();
#endif
        }

        private void BindAdService()
        {
#if ADMOB
            this.Container.Bind<IAdService>().To<AdMobService>().AsTransient();
#endif
        }

        private void InstallSignals()
        {
            this.Container.DeclareSignal<BannerAdPresentedSignal>();
            this.Container.DeclareSignal<BannerAdDismissedSignal>();
            this.Container.DeclareSignal<BannerAdLoadedSignal>();
            this.Container.DeclareSignal<BannerAdLoadFailedSignal>();
            this.Container.DeclareSignal<BannerAdClickedSignal>();

            this.Container.DeclareSignal<InterstitialAdLoadedSignal>();
            this.Container.DeclareSignal<InterstitialAdLoadFailedSignal>();
            this.Container.DeclareSignal<InterstitialAdClickedSignal>();
            this.Container.DeclareSignal<InterstitialAdDisplayedFailedSignal>();
            this.Container.DeclareSignal<InterstitialAdDisplayedSignal>();
            this.Container.DeclareSignal<InterstitialAdClosedSignal>();
            this.Container.DeclareSignal<InterstitialAdCalledSignal>();
            this.Container.DeclareSignal<InterstitialAdEligibleSignal>();

            this.Container.DeclareSignal<RewardedAdLoadedSignal>();
            this.Container.DeclareSignal<RewardedAdLoadFailedSignal>();
            this.Container.DeclareSignal<RewardedAdClickedSignal>();
            this.Container.DeclareSignal<RewardedAdDisplayedSignal>();
            this.Container.DeclareSignal<RewardedAdCompletedSignal>();
            this.Container.DeclareSignal<RewardedSkippedSignal>();
            this.Container.DeclareSignal<RewardedAdEligibleSignal>();
            this.Container.DeclareSignal<RewardedAdCalledSignal>();
            this.Container.DeclareSignal<RewardedAdOfferSignal>();
            this.Container.DeclareSignal<RewardedAdClosedSignal>();
            this.Container.DeclareSignal<RewardedAdShowFailedSignal>();
        }
    }
}
#endif