namespace UITemplate.Photon.Scripts
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Scripts.Extension;
    using UnityEngine;
    using Zenject;

    public class UITemplatePhotonService : IInitializable
    {
        private readonly IGameAssets gameAssets;
        
        public void Initialize() {  }
        
        public UITemplatePhotonService(IGameAssets gameAssets)
        {
            this.gameAssets = gameAssets;
        }

        private PhotonController photonController;
        
        public Action<List<RoomInfo>> UpdateListRoom;
        
        public UniTask Init()
        {
            Debug.Log($"Photon status: {PhotonNetwork.NetworkClientState.ToString()}");

            if (this.photonController == null)
            {
                // this.photonController = FindObjectOfType<PhotonController>();
                
                
                this.photonController.OnConnectedToMasterAction += () =>
                {
                    Debug.Log("Connected to master");
                    PhotonNetwork.JoinLobby();
                };
                
                this.photonController.OnRoomListUpdateAction += (rooms) =>
                {
                    Debug.Log($"Room list update: {rooms.Count}");
                    this.UpdateListRoom(rooms);
                };
            }

            return UniTask.CompletedTask;
        }

        public void CreateRoom(string nameRoom, int playerCount)
        {
            var options = new RoomOptions
            {
                MaxPlayers = playerCount,
            };
            PhotonNetwork.CreateRoom(nameRoom, options);
            Debug.Log($"Create room with name: {nameRoom}");
        }

        public void JoinRoom(string nameRoom) { }

        public bool CheckHasRoom(string nameRoom) { return false; }

        public void LeaveRoom() { }

        public void JoinLobby() { PhotonNetwork.JoinLobby(); }

        public void OutLobby() { PhotonNetwork.LeaveLobby(); }

        
    }
}