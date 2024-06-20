#if ADS_SERVICE
namespace UITemplate.AdsService
{
    using UITemplate.AdsService.Scripts.Base;
    using Zenject;

    public class AdSetting : IInitializable
    {
        public AdSetting(AdNetworkBaseSetting adNetworkBaseSetting)
        {
            this.AdNetworkBaseSetting = adNetworkBaseSetting;
        }
        public readonly AdNetworkBaseSetting AdNetworkBaseSetting;

        public void Initialize()
        {
            
        }
    }
}
#endif