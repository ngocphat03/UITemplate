#if PHOTON
namespace UITemplate.Photon.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using UITemplate.Photon.Signals;
    using UITemplate.Scripts.Extension;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    public class UITemplatePhotonService : MonoBehaviourPunCallbacks, IInitializable
    {
        [Inject] public IGameAssets GameAssets;
        [Inject] public SignalBus   SignalBus;

        public string NamePlayer => PhotonNetwork.NickName;
        public List<Player> ListPlayerInRoom => PhotonNetwork.PlayerList.ToList();

        public List<RoomInfo> ListRoom = new ();
        
        public bool IsMasterRoom => PhotonNetwork.IsMasterClient;
        
        
        
        // public List<RoomProfile> rooms = new ();

        public void Initialize()
        {
            Debug.Log($"Photon status: {PhotonNetwork.NetworkClientState.ToString()}");
            this.LoginPhoton();
            this.GetCurrentContainer().Inject(this);
        }

        private void LoginPhoton()
        {
            PhotonNetwork.LocalPlayer.NickName = $"$Player {Random.Range(0, 100)}";
            PhotonNetwork.ConnectUsingSettings();
        }
        
        private void LogoutPhoton()
        {
            PhotonNetwork.Disconnect();
        }
        
    public virtual void Create(string nameRoom)
    {
        Debug.Log( ": Create Room " + nameRoom);
        PhotonNetwork.CreateRoom(nameRoom);
    }

    public virtual void Join(string nameRoom)
    {
        Debug.Log(": Join Room " + nameRoom);
        PhotonNetwork.JoinRoom(nameRoom);
        this.ClearRoomProfileUI();
    }

    public virtual void Leave()
    {
        Debug.Log(": Leave Room");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        this.SignalBus.Fire<OnCreateRoomSignal>();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate" + roomList.Count);
        this.ListRoom = roomList;
        this.SignalBus.Fire(new OnUpdateRoomSignal(listRoom: this.ListRoom));
    }

    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        // RoomProfile roomProfile;
        //
        // roomProfile = this.RoomByName(roomInfo.Name);
        // if (roomProfile != null) return;
        //
        // roomProfile = new RoomProfile
        // {
        //     name = roomInfo.Name
        // };
        // this.rooms.Add(roomProfile);

    }

    protected virtual void UpdateRoomProfileUI()
    {
        // this.ClearRoomProfileUI();
        //
        // foreach (RoomProfile profile in this.rooms)
        // {
        //     UIRoomProfile uiRoomProfile = Instantiate(this.roomPrefab);
        //     uiRoomProfile.SetRoomProfile(profile);
        //     uiRoomProfile.transform.SetParent(this.roomContent);
        // }
    }

    protected virtual void ClearRoomProfileUI()
    {
        // foreach (Transform child in this.roomContent)
        // {
        //     Destroy(child.gameObject);
        // }
    }

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        // RoomProfile roomProfile = this.RoomByName(roomInfo.Name);
        // if (roomProfile == null) return;
        // this.rooms.Remove(roomProfile);
    }

    protected virtual RoomProfile RoomByName(string name)
    {
        // if (this.rooms == null) return null;
        // foreach (RoomProfile roomProfile in this.rooms)
        // {
        //     if (roomProfile.name == name) return roomProfile;
        // }
        return null;
    }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

        // public void CreateRoom(string nameRoom, int playerCount)
        // {
        //     var options = new RoomOptions
        //     {
        //         MaxPlayers = 10,
        //         IsVisible = true,
        //         IsOpen = true,
        //         PublishUserId = true,
        //     };
        //     PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        //     PhotonNetwork.CreateRoom(nameRoom, options);
        //     Debug.Log($"Create room with name \"{nameRoom}\" and max player: \"{playerCount}\"");
        // }
        //
        // public void JoinRoom(string nameRoom) { PhotonNetwork.JoinRoom(nameRoom); }
        //
        //
        // public void LeaveRoom()
        // {
        //     // if (this.IsMasterRoom)
        //     {
        //         PhotonNetwork.CurrentRoom.RemovedFromList = true;
        //         PhotonNetwork.CurrentRoom.IsOpen = false;
        //         PhotonNetwork.CurrentRoom.IsVisible = false;
        //         foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        //         {
        //             PhotonNetwork.CloseConnection(player);
        //         }
        //
        //     }
        //     //
        //     // PhotonNetwork.LeaveRoom();
        // }
        //
        // public void RemoveRoom() {}
        //
        // public void StartGame() {}
        //
        // public void JoinLobbym() { PhotonNetwork.JoinLobby(); }
        //
        // public void OutLobbym() { PhotonNetwork.LeaveLobby(); }
        //
        // #region Pun Callbacks
        //
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }
        //
        // public override void OnJoinedLobby() { Debug.Log("Joined lobby"); }
        //
        // // public override void OnCreatedRoom()
        // // {
        // //     Debug.Log("Room created successfully!");
        // //     // this.signalBus.Fire<OnCreateRoomSignal>();
        // //     this.OnCreateRoom?.Invoke();
        
        // // }
        //
        // public override void OnConnected() { }
        //
        // public override void OnDisconnected(DisconnectCause cause) { }
        //
        // public override void OnErrorInfo(ErrorInfo errorInfo) { }
        //
        // public override void OnLeftLobby() { }
        //
        //
        // public override void OnJoinRoomFailed(short returnCode, string message) { }
        //
        //
        // // public override void OnRoomListUpdate(List<RoomInfo> roomList)
        // // {
        // //     Debug.Log("Room list updated");
        // //     this.ListRoom = roomList.Where(item => item.RemovedFromList == false).ToList();
        // //     
        // //     // this.UpdateListRoom?.Invoke(roomList);
        // // }
        //
        // public override void OnPlayerEnteredRoom(Player newPlayer)
        // {
        //     Debug.Log($"Player {newPlayer.NickName} entered room");
        //     this.OnPlayerJoinedRoom?.Invoke(newPlayer);
        // }
        //
        // public override void OnPlayerLeftRoom(Player otherPlayer)
        // {
        //     Debug.Log($"Player {otherPlayer.NickName} left room");
        //     this.OnPlayerLeftTheRoom?.Invoke(otherPlayer);
        // }
        //
        // // protected virtual void RoomAdd(RoomInfo roomInfo)
        // // {
        // //     var roomProfile = new RoomProfile
        // //     {
        // //         name = roomInfo.Name
        // //     };
        // //     this.rooms.Add(roomProfile);
        // // }
        // // protected virtual void RoomRemove(RoomInfo roomInfo)
        // // {
        // //     var roomProfile = this.RoomByName(roomInfo.Name);
        // //     if (roomProfile == null) return;
        // //     this.rooms.Remove(roomProfile);
        // // }
        // // protected virtual RoomProfile RoomByName(string nameRoom)
        // // {
        // //     return this.rooms.FirstOrDefault(roomProfile => roomProfile.name == nameRoom);
        // // }
        //
        // #endregion
    }
}
#endif