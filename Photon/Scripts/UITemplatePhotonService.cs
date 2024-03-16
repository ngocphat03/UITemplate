#if PHOTON
namespace UITemplate.Photon.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Scripts.Extension;
    using UnityEngine;
    using Zenject;

    public class UITemplatePhotonService : MonoBehaviourPunCallbacks, IInitializable
    {
        private readonly IGameAssets gameAssets;

        public UITemplatePhotonService(IGameAssets gameAssets) { this.gameAssets = gameAssets; }

        public Action<List<RoomInfo>> UpdateListRoom;
        public Action<Player> OnPlayerJoinedRoom;
        public Action<Player> OnPlayerLeftTheRoom;
        
        public string NamePlayer => PhotonNetwork.NickName;
        public List<Player> ListPlayerInRoom => PhotonNetwork.PlayerList.ToList();

        public void Initialize()
        {
            Debug.Log($"Photon status: {PhotonNetwork.NetworkClientState.ToString()}");
            this.LoginPhoton();
        }

        private void LoginPhoton()
        {
            PhotonNetwork.NickName = "Player 1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public UniTask Init() { return UniTask.CompletedTask; }

        public void CreateRoom(string nameRoom, int playerCount)
        {
            var options = new RoomOptions
            {
                MaxPlayers = 10,
                IsVisible = true,
                IsOpen = true,
                PublishUserId = true,
            };
            PhotonNetwork.CreateRoom(nameRoom, options);
            Debug.Log($"Create room with name \"{nameRoom}\" and max player: \"{playerCount}\"");
        }

        public void JoinRoom(string nameRoom) { PhotonNetwork.JoinRoom(nameRoom); }
        
        public bool IsMasterRoom => PhotonNetwork.IsMasterClient;

        public void LeaveRoom() { }

        public void JoinLobbym() { PhotonNetwork.JoinLobby(); }

        public void OutLobbym() { PhotonNetwork.LeaveLobby(); }

        #region Pun Callbacks

        public override void OnConnectedToMaster() { PhotonNetwork.JoinLobby(); }

        public override void OnJoinedLobby() { Debug.Log("Joined lobby"); }

        public override void OnCreatedRoom() { Debug.Log("Room created successfully!"); }

        public override void OnCreateRoomFailed(short returnCode, string message) { Debug.LogError("Room creation failed: " + message); }

        public override void OnConnected() { }

        public override void OnDisconnected(DisconnectCause cause) { }

        public override void OnErrorInfo(ErrorInfo errorInfo) { }

        public override void OnLeftLobby() { }

        public override void OnLeftRoom() { }

        public override void OnJoinRoomFailed(short returnCode, string message) { }

        public override void OnJoinedRoom() { Debug.Log("Join room successfully!"); }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room list updated");
            this.UpdateListRoom?.Invoke(roomList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} entered room");
            this.OnPlayerJoinedRoom?.Invoke(newPlayer);
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} left room");
            this.OnPlayerLeftTheRoom?.Invoke(otherPlayer);
        }

        #endregion
    }
}
#endif