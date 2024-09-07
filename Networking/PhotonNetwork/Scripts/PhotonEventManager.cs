#if PHOTON

namespace AXitUnityTemplate.Networking.PhotonNetwork.Scripts
{
    using System;
    using Zenject;
    using global::Photon.Pun;
    using global::Photon.Realtime;
    using ExitGames.Client.Photon;
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

        public void SendEvent(byte eventCode, object data, RaiseEventOptions raiseEventOptions = default, SendOptions sendOption = default)
        {
            sendOption = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(eventCode, data, raiseEventOptions, sendOption);
        }

        public void Dispose() { PhotonNetwork.RemoveCallbackTarget(this); }
    }
}
#endif