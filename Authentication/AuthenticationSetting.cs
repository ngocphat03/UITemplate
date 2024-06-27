namespace UITemplate.Authentication
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AuthenticationSetting", menuName = "Authentication/AuthenticationSetting")]
    public class AuthenticationSetting : ScriptableObject
    {
        public string GoogleAPI           { get; set; }
        public string FirebaseDatabaseURL { get; set; }
    }
}