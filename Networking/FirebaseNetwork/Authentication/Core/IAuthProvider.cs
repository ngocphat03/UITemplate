namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Core
{
    using System;

    public interface IAuthProvider
    {
        public void Login(Action onSuccess, Action onFailure);

        // void SignIn(System.Action<UserInfo> onSuccess, System.Action<string> onFailure);
        //
        // /// <summary>
        // /// Signs out the current user.
        // /// </summary>
        // void SignOut();
        //
        // /// <summary>
        // /// Returns whether the user is currently signed in with this provider.
        // /// </summary>
        // /// <returns>True if the user is signed in, otherwise false.</returns>
        // bool IsSignedIn();
        //
        // /// <summary>
        // /// Retrieves the currently signed-in user's information.
        // /// </summary>
        // /// <returns>User information if signed in, otherwise null.</returns>
        // UserInfo GetUserInfo();
    }
}