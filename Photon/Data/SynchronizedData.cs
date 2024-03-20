namespace UITemplate.Photon.Data
{
    using System.Collections.Generic;
    using global::Photon.Realtime;

    public class SynchronizedData
    {
        public List<PlayerData> Players { get; set; }
}

    public class PlayerData
    {
        public Player Player { get; set; }
        public string Name   { get; set; }
        public int    Score  { get; set; }
        public bool   IsTurn { get; set; }
    }
}