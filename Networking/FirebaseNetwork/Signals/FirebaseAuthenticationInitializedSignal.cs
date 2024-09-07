namespace AXitUnityTemplate.Networking.FirebaseNetwork.Signals
{
    using Firebase.Auth;
    using Firebase.Database;

    public class FirebaseAuthenticationInitializedSignal
    {
        public DatabaseReference DatabaseReference { get; set; }
        public FirebaseAuth      FirebaseAuth      { get; set; }
        public FirebaseUser      FirebaseUser      { get; set; }
        
        public FirebaseAuthenticationInitializedSignal(FirebaseAuth firebaseAuth, FirebaseUser firebaseUser, DatabaseReference databaseReference)
        {
            this.FirebaseAuth      = firebaseAuth;
            this.FirebaseUser      = firebaseUser;
            this.DatabaseReference = databaseReference;
        }
    }
}