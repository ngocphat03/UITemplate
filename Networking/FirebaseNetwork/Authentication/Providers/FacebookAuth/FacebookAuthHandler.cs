#if FACEBOOK
namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Providers.FacebookAuth
{
    using System;
    using Zenject;
    using UnityEngine;
    using Firebase.Auth;
    using Facebook.Unity;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Core;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Database.UserProfile;

    public class FacebookAuthHandler : IAuthProvider, IInitializable
    {
        private readonly AuthenticationService authenticationService;
        private readonly UserProfileManager    userProfileManager;

        private FirebaseAuth Auth => this.authenticationService.Auth;
        private FirebaseUser User => this.authenticationService.User;
        private string       fbUserId;

        public FacebookAuthHandler(AuthenticationService authenticationService,
                                   UserProfileManager    userProfileManager)
        {
            this.authenticationService = authenticationService;
            this.userProfileManager    = userProfileManager;
        }

        public void Initialize()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(() =>
                {
                    if (FB.IsInitialized)
                        FB.ActivateApp();
                    else
                        Debug.Log("Failed to initialize the Facebook SDK");
                });
            }
            else
            {
                FB.ActivateApp();
            }
        }

        public void Login(Action onSuccess, Action onFailure)
        {
            var permissions = new List<string> { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(permissions, AuthCallback);

            return;

            async void AuthCallback(ILoginResult result)
            {
                if (!FB.IsLoggedIn)
                {
                    onFailure?.Invoke();
                    Debug.Log("User cancelled Facebook login");

                    return;
                }

                var aToken     = AccessToken.CurrentAccessToken;
                var credential = FacebookAuthProvider.GetCredential(aToken.TokenString);
                try
                {
                    this.fbUserId   = result.ResultDictionary["user_id"].ToString();
                    var taskResult = await this.Auth.SignInAndRetrieveDataWithCredentialAsync(credential);
                    this.authenticationService.SetUser(taskResult.User);
                    await this.AsyncData();
                    onSuccess?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + ex);
                    onFailure?.Invoke();
                }
            }
        }

        private async Task AsyncData()
        {
            Debug.LogError( this.fbUserId);
            var user = await this.userProfileManager.LoadUserProfile(this.User.UserId);
            if (user == null)
            {
                var listFriendsOnFacebook = await this.GetFacebookFriendsAsync();
                // await this.userProfileManager.AddUserProfile(new User
                // {
                //     UserId     = this.User.UserId,
                //     FacebookId = this.fbUserId,
                // });
            }
        }

        private async Task<Dictionary<string, string>> GetFacebookFriendsAsync()
        {
            const string query      = "/me/friends";
            var          listFriend = new Dictionary<string, string>();
            var          tcs        = new TaskCompletionSource<Dictionary<string, string>>();

            FB.API(query, HttpMethod.GET, result =>
            {
                if (result.Error != null)
                {
                    Debug.Log("Error getting friends: " + result.Error);
                    tcs.SetException(new Exception(result.Error));

                    return;
                }

                if (result.ResultDictionary["data"] is List<object> friendsList)
                {
                    foreach (var friend in friendsList)
                    {
                        if (friend is not Dictionary<string, object> friendDict) continue;
                        var id   = friendDict["id"].ToString();
                        var name = friendDict["name"].ToString();
                        listFriend.Add(id, name);
                        Debug.Log("Friend ID: " + id + " Name: " + name);
                    }
                }

                tcs.SetResult(listFriend);
            });

            return await tcs.Task;
        }
    }
}
#endif