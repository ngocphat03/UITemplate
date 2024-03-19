namespace UITemplate.Photon.Signals
{
    public class NotifyForAllPlayerSignal
    {
        public int ActorNumber { get; set; }
        
        public NotifyForAllPlayerSignal(int actorNumber)
        {
            this.ActorNumber = actorNumber;
        }
        public NotifyForAllPlayerSignal()
        {
        }
    }
}