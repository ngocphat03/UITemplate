#if ADS_SERVICE
namespace UITemplate.AdsService.Scripts.Base
{
    using UnityEngine;

    public class AdNetworkBaseSetting : ScriptableObject
    {
        public string AppId;
        public string BannerId;
        public string InterstitialId;
        public string RewardedId;
    }
}
#endif