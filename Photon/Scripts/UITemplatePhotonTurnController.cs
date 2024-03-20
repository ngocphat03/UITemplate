#if PHOTON

namespace UITemplate.Photon.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using Controller;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Photon.Signals;
    using UnityEngine;
    using Zenject;

    public class UITemplatePhotonTurnController : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] public SignalBus SignalBus;

        public List<Player> ListPlayerInRoom => PhotonNetwork.PlayerList.ToList();

        public int CurrentTurn { get; private set; }

        public Player PlayerCurrentTurn => this.ListPlayerInRoom[this.CurrentTurn];

        public void Initialize()
        {
            this.gameObject.AddComponent<PhotonView>();
            this.SignalBus.Subscribe<StartOnlineGameSignal>(this.StartGame);
        }

        private void StartGame(StartOnlineGameSignal signal) { this.SetTurn(signal.ActorTurn); }

        public void SetTurn(int indexPlayer) { this.CurrentTurn = indexPlayer; }

        public void NextTurn()
        {
            this.CurrentTurn = this.CurrentTurn == 0 ? 1 : 0;

            this.NotifyForCurrentPlayer();
        }

        public void NotifyForCurrentPlayer()
        {
            Debug.Log("NotifyForCurrentPlayer");
            this.SignalBus.Fire(new NotifyForAllPlayerSignal());
        }
    }
}
#endif