namespace UITemplate.Photon.Signals
{
    using System.Collections.Generic;
    using global::Photon.Realtime;

    public class OnUpdateRoomSignal
    {
        public List<RoomInfo> ListRoom;
        public OnUpdateRoomSignal(List<RoomInfo> listRoom) { this.ListRoom = listRoom; }
    }
}