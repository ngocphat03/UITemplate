namespace UITemplate.Photon.Data
{
    using UITemplate.Photon.Scripts;

    public class DataRealTimeManager
    {
        private readonly UITemplatePhotonService photonService;
        public DataRealTimeManager(UITemplatePhotonService photonService) { this.photonService = photonService; }
        
        public SynchronizedData SynchronizedData { get; private set; }
        
        public void SetSynchronizedData(SynchronizedData synchronizedData)
        {
            this.SynchronizedData = synchronizedData;
        }
        
        
        
        public void SetTurn(int actorNumber) { this.SynchronizedData.Players.Find(player => player.Player.ActorNumber == actorNumber).IsTurn = true; }

    }
}