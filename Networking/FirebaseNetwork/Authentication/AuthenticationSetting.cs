namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AuthenticationSetting", menuName = "Authentication/AuthenticationSetting")]
    public class AuthenticationSetting : ScriptableObject
    {
        public string googleAPI;
        public string firebaseDatabaseURL;
    }
}