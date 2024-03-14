namespace UITemplate.Photon.Scripts
{
    using System;
    using System.Collections.Generic;
    using global::Photon.Pun;
    using global::Photon.Realtime;

    public class PhotonController : MonoBehaviourPunCallbacks
    {
        public Action                  OnJoinedLobbyAction;
        public Action                  OnConnectedAction;
        public Action<DisconnectCause> OnDisconnectedAction;
        public Action                  OnCreatedRoomAction;
        public Action<ErrorInfo>       OnErrorInfoAction;
        public Action                  OnLeftLobbyAction;
        public Action                  OnLeftRoomAction;
        public Action<short, string>   OnJoinRoomFailedAction;
        public Action                  OnJoinedRoomAction;
        public Action<short, string>   OnCreateRoomFailedAction;
        public Action                  OnConnectedToMasterAction;
        public Action<List<RoomInfo>>  OnRoomListUpdateAction;

        public static PhotonController Instance;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            this.OnJoinedLobbyAction?.Invoke();
        }

        public override void OnConnected()
        {
            base.OnConnected();
            this.OnConnectedAction?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            this.OnDisconnectedAction?.Invoke(cause);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            this.OnCreatedRoomAction?.Invoke();
        }

        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            base.OnErrorInfo(errorInfo);
            this.OnErrorInfoAction?.Invoke(errorInfo);
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            this.OnLeftLobbyAction?.Invoke();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            this.OnLeftRoomAction?.Invoke();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            this.OnJoinRoomFailedAction?.Invoke(returnCode, message);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            this.OnJoinedRoomAction?.Invoke();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            this.OnCreateRoomFailedAction?.Invoke(returnCode, message);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            this.OnConnectedToMasterAction?.Invoke();
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            this.OnRoomListUpdateAction?.Invoke(roomList);
        }
    }
}