
#if PHOTON
namespace UITemplate.Photon.Signals
{
    public class InYourTurnSignal
    {
        public int ActorNumber { get; set; }
        
        public InYourTurnSignal(int actorNumber)
        {
            this.ActorNumber = actorNumber;
        }
    }
}
#endif
