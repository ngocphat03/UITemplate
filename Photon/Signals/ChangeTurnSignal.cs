namespace UITemplate.Photon.Signals
{
    public class ChangeTurnSignal
    {
        public int ActorNumber { get; set; }

        public ChangeTurnSignal(int actorNumber) { this.ActorNumber = actorNumber; }
    }
}