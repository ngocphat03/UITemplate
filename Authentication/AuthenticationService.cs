#if FIREBASE && AUTHENTICATION
namespace UITemplate.Authentication
{
    using System;
    using Zenject;
    using Google;
    using Firebase;
    using UnityEngine;
    using Firebase.Auth;
    using Firebase.Database;
    using Firebase.Extensions;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;

    public class AuthenticationService : IInitializable
    {
        public DependencyStatus  DependencyStatus  { get; private set; }
        public FirebaseAuth      Auth              { get; private set; }
        public FirebaseUser      User              { get; private set; }
        public DatabaseReference DatabaseReference { get; private set; }

        private GoogleSignInConfiguration googleSignInConfiguration;
        private AuthenticationSetting     authenticationSetting;

        public async void Initialize()
        {
            // Load the authentication setting
            this.authenticationSetting = Resources.Load<AuthenticationSetting>(nameof(AuthenticationSetting));

            if (this.authenticationSetting == null) throw new Exception($"Cannot find AuthenticationSetting in Resources folder");

            await this.CheckAndFixDependenciesAsync();

            this.googleSignInConfiguration = new GoogleSignInConfiguration
            {
                WebClientId    = this.authenticationSetting.googleAPI,
                RequestIdToken = true,
            };

            if (this.DependencyStatus == DependencyStatus.Available)
            {
                this.Auth              = FirebaseAuth.DefaultInstance;
                this.DatabaseReference = FirebaseDatabase.GetInstance(this.authenticationSetting.firebaseDatabaseURL).RootReference;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {this.DependencyStatus}");
            }
        }

        private async UniTask CheckAndFixDependenciesAsync()
        {
            var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
            this.DependencyStatus = await dependencyTask;
        }

        public async UniTask Login(string email, string password, Action onLoginSuccess = null, Action onLoginFailed = null)
        {
            if (this.Auth == null)
            {
                Debug.LogError("FirebaseAuth is not initialized.");

                return;
            }

            var loginTask = this.Auth.SignInWithEmailAndPasswordAsync(email, password);

            try
            {
                // Await the task to complete
                var loginResult = await loginTask;

                // User is now logged in, now get the result
                this.User = loginResult.User;
                onLoginSuccess?.Invoke();
                Debug.LogFormat("User signed in successfully: {0} ({1})", this.User.DisplayName, this.User.Email);
            }
            catch (Exception e)
            {
                // If there are errors handle them
                Debug.LogWarning($"Failed to register task with {e}");
                var firebaseEx = e.GetBaseException() as FirebaseException;
                // ReSharper disable once PossibleNullReferenceException
                var errorCode = (AuthError)firebaseEx.ErrorCode;

                var message = errorCode switch
                {
                    AuthError.MissingEmail    => "Missing Email",
                    AuthError.MissingPassword => "Missing Password",
                    AuthError.WrongPassword   => "Wrong Password",
                    AuthError.InvalidEmail    => "Invalid Email",
                    AuthError.UserNotFound    => "Account does not exist",
                    _                         => "Login Failed!",
                };
                Debug.LogError(message);
                onLoginFailed?.Invoke();
            }
        }

        public async UniTask Register(string email, string password, string username, Action onRegisterSuccess = null, Action onRegisterFalse = null)
        {
            try
            {
                //Call the Firebase auth create user function passing the email and password
                var registerTask = this.Auth.CreateUserWithEmailAndPasswordAsync(email, password);
                //Wait until the task completes
                await registerTask;

                if (registerTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                    var firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                    // ReSharper disable once PossibleNullReferenceException
                    var errorCode = (AuthError)firebaseEx.ErrorCode;

                    var message = errorCode switch
                    {
                        AuthError.MissingEmail      => "Missing Email",
                        AuthError.MissingPassword   => "Missing Password",
                        AuthError.WeakPassword      => "Weak Password",
                        AuthError.EmailAlreadyInUse => "Email Already In Use",
                        _                           => "Register Failed!"
                    };

                    Debug.LogError(message);
                    onRegisterFalse?.Invoke();
                }
                else
                {
                    this.User = registerTask.Result.User;

                    if (this.User != null)
                    {
                        //Create a user profile and set the username
                        var profile = new UserProfile { DisplayName = username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        var profileTask = this.User.UpdateUserProfileAsync(profile);
                        //Wait until the task completes
                        await profileTask;

                        if (profileTask.Exception != null)
                        {
                            //If there are errors handle them
                            Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                            var firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                            // ReSharper disable once PossibleNullReferenceException
                            var errorCode = (AuthError)firebaseEx.ErrorCode;

                            Debug.LogError($"Username Set Failed! {errorCode}");
                            onRegisterFalse?.Invoke();
                        }
                        else
                        {
                            Debug.Log("Username Set Successfully!");
                            onRegisterSuccess?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to register task with {e}");
                onRegisterFalse?.Invoke();
            }
        }

        public void Logout(Action onLogoutSuccess = null, Action onLogoutFailed = null)
        {
            try
            {
                this.Auth.SignOut();
                this.User = null;
                onLogoutSuccess?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to logout task with {e}");
                onLogoutFailed?.Invoke();
            }
        }

        public async UniTask LoginWithGoogle(Action onLoginSuccess = null, Action onLoginFailed = null)
        {
            try
            {
                GoogleSignIn.Configuration                = this.googleSignInConfiguration;
                GoogleSignIn.Configuration.UseGameSignIn  = false;
                GoogleSignIn.Configuration.RequestIdToken = true;
                GoogleSignIn.Configuration.RequestEmail   = true;

                var signInTask = GoogleSignIn.DefaultInstance.SignIn();
                await signInTask.ContinueWith(OnGoogleAuthenticatedFinished);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception during Google Sign-In: {ex.Message}");
                onLoginFailed?.Invoke();
            }

            return;

            void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Google Sign-In failed: " + task.Exception);
                    onLoginFailed?.Invoke();
                }
                else if (task.IsCanceled)
                {
                    Debug.LogError("Google Sign-In was canceled.");
                    onLoginFailed?.Invoke();
                }
                else
                {
                    var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

                    this.Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task1 =>
                    {
                        if (task1.IsCanceled)
                        {
                            Debug.LogError("SignInWithCredentialAsync was canceled.");
                            onLoginFailed?.Invoke();

                            return;
                        }

                        if (task1.IsFaulted)
                        {
                            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task1.Exception);
                            onLoginFailed?.Invoke();

                            return;
                        }

                        this.User = this.Auth.CurrentUser;
                        onLoginSuccess?.Invoke();
                    });
                }
            }
        }
    }
}
#endif