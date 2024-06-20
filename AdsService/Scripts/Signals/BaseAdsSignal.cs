#if ADS_SERVICE
namespace UITemplate.AdsService.Scripts.Signals
{
    public class BaseAdsSignal
    {
        public string Placement;

        public BaseAdsSignal(string placement) { this.Placement = placement; }
    }
}
#endif