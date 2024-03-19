namespace UITemplate.Photon.Signals
{
    public class StartOnlineGameSignal
    {
        public int ActorTurn { get; set; }
        public StartOnlineGameSignal(int actorTurn)
        {
            this.ActorTurn = actorTurn;
        }
    }
}