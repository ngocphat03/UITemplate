namespace UITemplate.Photon.Data
{
    using System;
    using Controller;
    using global::Signals;
    using UITemplate.Photon.Scripts;
    using UnityEngine;
    using Zenject;

    public class DataRealTimeManager : IInitializable
    {
        private readonly UITemplatePhotonRoomService photonRoomService;
        private readonly SignalBus               signalBus;

        public DataRealTimeManager(UITemplatePhotonRoomService photonRoomService, SignalBus signalBus)
        {
            this.photonRoomService     = photonRoomService;
            this.signalBus         = signalBus;
        }

        public SynchronizedData SynchronizedData { get; private set; }
        
        public Action<int, int> OnUpdateDataInUI { get; set; }
        public Action <int> UpdatePlayerScore { get; set; }

        public void Initialize() { this.SynchronizedData = new SynchronizedData(); }

        // NOTE: AddScore and AddOpponentScore do not run at the same time on the same device
        public void AddScore(int newScore)
        {
            this.SynchronizedData.MyScore += newScore;
            
            this.OnUpdateDataInUI?.Invoke(this.SynchronizedData.MyScore, this.SynchronizedData.OpponentScore);
            this.UpdatePlayerScore?.Invoke(newScore);
            // this.signalBus.Fire<UpdateDataInUISignal>();
            // this.signalBus.Fire(new UpdateScorePlayerSignal(newScore));
        }

        public void AddOpponentScore(int newScore)
        {
            this.SynchronizedData.OpponentScore += newScore;
            Debug.Log($"Current score: {this.SynchronizedData.OpponentScore}");

            this.OnUpdateDataInUI?.Invoke(this.SynchronizedData.MyScore, this.SynchronizedData.OpponentScore);
            // this.signalBus.Fire(new UpdateDataInUISignal(this.SynchronizedData.MyScore, this.SynchronizedData.OpponentScore));
        }
    }
}