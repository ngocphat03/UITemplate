#if PHOTON

namespace AXitUnityTemplate.Networking.Photon.Scripts
{
    using System;
    using Zenject;
    using global::Photon.Pun;
    using ExitGames.Client.Photon;
    using global::Photon.Realtime;
    using System.Collections.Generic;

    public class PhotonEventManager : IInitializable, IDisposable, IOnEventCallback
    {
        private readonly Dictionary<byte, Action<object[]>> eventDictionary = new();

        public void Initialize() { PhotonNetwork.AddCallbackTarget(this); }

        public void OnEvent(EventData photonEvent)
        {
            if (!this.eventDictionary.TryGetValue(photonEvent.Code, out var action)) return;
            action?.Invoke((object[])photonEvent.CustomData);
        }

        public void AddEvent(byte eventCode, Action<object[]> action) { this.eventDictionary.TryAdd(eventCode, action); }

        public void RemoveEvent(byte eventCode) { this.eventDictionary.Remove(eventCode); }

        public void SendEvent(byte eventCode, object data, ReceiverGroup receiverGroup = ReceiverGroup.All, SendOptions sendOptions = default)
        {
            sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(eventCode, data, new RaiseEventOptions { Receivers = receiverGroup }, sendOptions);
        }

        public void Dispose() { PhotonNetwork.RemoveCallbackTarget(this); }
    }
}
#endif