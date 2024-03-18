#if PHOTON

namespace UITemplate.Photon.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Photon.Signals;
    using Zenject;

    public class UITemplatePhotonTurnController : MonoBehaviourPunCallbacks, IInitializable
    {
        private readonly SignalBus signalBus;

        public UITemplatePhotonTurnController(SignalBus signalBus) { this.signalBus = signalBus; }

        public List<Player> ListPlayerInRoom => PhotonNetwork.PlayerList.ToList();

        public int CurrentTurn { get; private set; }

        public void Initialize() { }

        public void SetTurn(int indexPlayer) { this.CurrentTurn = indexPlayer; }

        public void NextTurn()
        {
            this.CurrentTurn++;
            if (this.CurrentTurn >= this.ListPlayerInRoom.Count)
            {
                this.CurrentTurn = 0;
            }
        }

        public void NotifyForCurrentPlayer() { this.photonView.RPC(nameof(this.NotifyPlayerTurn), this.ListPlayerInRoom[this.CurrentTurn]); }

        [PunRPC]
        private void NotifyPlayerTurn() { this.signalBus.Fire<InYourTurnSignal>(); }
    }
}
#endif