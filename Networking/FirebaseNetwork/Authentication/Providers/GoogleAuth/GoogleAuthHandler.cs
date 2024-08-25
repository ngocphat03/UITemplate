namespace AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Providers.GoogleAuth
{
    using System;
    using Google;
    using Zenject;
    using UnityEngine;
    using Firebase.Auth;
    using Firebase.Extensions;
    using System.Threading.Tasks;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Authentication.Core;

    public class GoogleAuthHandler : IAuthProvider, IInitializable
    {
        private readonly AuthenticationService authenticationService;

        private GoogleSignInConfiguration googleSignInConfiguration;
        private AuthenticationSetting     authenticationSetting;
        private FirebaseAuth              Auth => this.authenticationService.Auth;

        public GoogleAuthHandler(AuthenticationService authenticationService) { this.authenticationService = authenticationService; }

        public void Initialize()
        {
            this.authenticationSetting = Resources.Load<AuthenticationSetting>(nameof(AuthenticationSetting));

            if (this.authenticationSetting == null) throw new Exception($"Cannot find AuthenticationSetting in Resources folder");
            this.googleSignInConfiguration = new GoogleSignInConfiguration
            {
                WebClientId    = this.authenticationSetting.googleAPI,
                RequestIdToken = true,
            };
        }

        public async void Login(Action onSuccess, Action onFailure)
        {
            GoogleSignIn.Configuration                = this.googleSignInConfiguration;
            GoogleSignIn.Configuration.UseGameSignIn  = false;
            GoogleSignIn.Configuration.RequestIdToken = true;

            await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);

            return;

            async void OnAuthenticationFinished(Task<GoogleSignInUser> task)
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Faulted");
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("Cancelled");
                }
                else
                {
                    var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

                    await this.Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(tk =>
                    {
                        if (tk.IsCanceled)
                        {
                            Debug.LogError("SignInWithCredentialAsync was canceled.");
                            onFailure?.Invoke();

                            return;
                        }

                        if (tk.IsFaulted)
                        {
                            Debug.LogError("SignInWithCredentialAsync encountered an error: " + tk.Exception);
                            onFailure?.Invoke();

                            return;
                        }

                        onSuccess?.Invoke();
                    });
                }
            }
        }
    }
}