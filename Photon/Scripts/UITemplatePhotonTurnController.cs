#if PHOTON

namespace UITemplate.Photon.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using Zenject;

    public class UITemplatePhotonTurnController : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject]
        public SignalBus SignalBus;

        public List<Player> ListPlayerInRoom => PhotonNetwork.PlayerList.ToList();

        public int ActorNumber { get; private set; }

        public Player PlayerCurrentTurn => this.ListPlayerInRoom.Find(player => player.ActorNumber == this.ActorNumber);

        public void Initialize() { this.gameObject.AddComponent<PhotonView>(); }

        public void SetTurn(int indexPlayer) { this.ActorNumber = indexPlayer; }

        public void NextTurn()
        {
            if (this.ActorNumber == this.ListPlayerInRoom[0].ActorNumber)
            {
                this.ActorNumber = this.ListPlayerInRoom[1].ActorNumber;
            }
            else
            {
                this.ActorNumber = this.ListPlayerInRoom[0].ActorNumber;
            }
        }
    }
}
#endif