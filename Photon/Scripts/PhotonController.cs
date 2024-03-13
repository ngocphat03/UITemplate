namespace UITemplate.Photon.Scripts
{
    using System;
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

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            this.OnJoinedLobbyAction();
        }

        public override void OnConnected()
        {
            base.OnConnected();
            this.OnConnectedAction();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            this.OnDisconnectedAction(cause);
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            this.OnCreatedRoomAction();
        }

        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            base.OnErrorInfo(errorInfo);
            this.OnErrorInfoAction(errorInfo);
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            this.OnLeftLobbyAction();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            this.OnLeftRoomAction();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            this.OnJoinRoomFailedAction(returnCode, message);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            this.OnJoinedRoomAction();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            this.OnCreateRoomFailedAction(returnCode, message);
        }
    }
}