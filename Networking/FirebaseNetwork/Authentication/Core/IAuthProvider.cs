namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Core
{
    using System;

    public interface IAuthProvider
    {
        public void Login(Action onSuccess, Action onFailure);
    }
}