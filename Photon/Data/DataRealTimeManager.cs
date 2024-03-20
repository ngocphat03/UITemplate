namespace UITemplate.Photon.Data
{
    using global::Signals;
    using UITemplate.Photon.Scripts;
    using Zenject;

    public class DataRealTimeManager : IInitializable
    {
        private readonly UITemplatePhotonService photonService;
        private readonly SignalBus               signalBus;

        public DataRealTimeManager(UITemplatePhotonService photonService, SignalBus signalBus)
        {
            this.photonService = photonService;
            this.signalBus     = signalBus;
        }

        public SynchronizedData SynchronizedData { get; private set; }

        public void Initialize() { this.SynchronizedData = new SynchronizedData(); }

        public void ChangeScore(int newScore, int actorPlayer)
        {
            if (actorPlayer == this.photonService.ActorNumber)
                this.SynchronizedData.MyScore += newScore;
            else
                this.SynchronizedData.OpponentScore += newScore;
            
            this.signalBus.Fire(new UpdateDataInUISignal(this.SynchronizedData.MyScore, this.SynchronizedData.OpponentScore));
        }
    }
}