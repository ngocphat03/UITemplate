#if PHOTON
namespace UITemplate.Photon.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Photon.Signals;
    using UITemplate.Scripts.Extension;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    public class UITemplatePhotonRoomService : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] public IGameAssets GameAssets;

        [Inject] public SignalBus SignalBus;

        public string       NamePlayer         => PhotonNetwork.LocalPlayer.NickName;
        public string       OpponentNamePlayer => PhotonNetwork.PlayerList.FirstOrDefault(x => x.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)?.NickName;
        public List<Player> ListPlayerInRoom   => PhotonNetwork.PlayerList.ToList();

        public List<RoomInfo> ListRoom = new();

        public bool IsMasterRoom => PhotonNetwork.IsMasterClient;
        public int  ActorNumber  => PhotonNetwork.LocalPlayer.ActorNumber;

        public void Initialize()
        {
            Debug.Log($"Photon status: {PhotonNetwork.NetworkClientState.ToString()}");
            this.LoginPhoton();
            this.GetCurrentContainer().Inject(this);
        }

        private void LoginPhoton()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LocalPlayer.NickName   = $"$Player {Random.Range(0, 100)}";
            PhotonNetwork.SendRate               = 60;
            PhotonNetwork.SerializationRate      = 5;
            PhotonNetwork.ConnectUsingSettings();
        }

        private void LogoutPhoton() { PhotonNetwork.Disconnect(); }

        public virtual void Create(string nameRoom)
        {
            Debug.Log(": Create Room " + nameRoom);
            PhotonNetwork.CreateRoom(nameRoom);
        }

        public virtual void Join(string nameRoom)
        {
            Debug.Log(": Join Room " + nameRoom);
            PhotonNetwork.JoinRoom(nameRoom);
        }

        public virtual void LeaveRoom()
        {
            Debug.Log(": Leave Room");
            PhotonNetwork.LeaveRoom();
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
            this.SignalBus.Fire<OnCreateRoomSignal>();
        }

        public override void OnJoinedRoom() { Debug.Log("OnJoinedRoom"); }

        public override void OnLeftRoom() { Debug.Log("OnLeftRoom"); }

        public override void OnCreateRoomFailed(short returnCode, string message) { Debug.Log("OnCreateRoomFailed: " + message); }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate" + roomList.Count);
            this.ListRoom = roomList;
            this.SignalBus.Fire(new OnUpdateRoomSignal(listRoom: this.ListRoom));
        }

        public void LoadPhotonScene(string sceneName) { PhotonNetwork.LoadLevel(sceneName); }

        #region Pun Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }

        #endregion
    }
}
#endif