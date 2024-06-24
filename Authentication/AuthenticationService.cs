#if FIREBASE && AUTHENTICATION
namespace UITemplate.Authentication
{
    using System;
    using Zenject;
    using Firebase;
    using UnityEngine;
    using Firebase.Auth;
    using Cysharp.Threading.Tasks;
    using Firebase.Database;

    public class AuthenticationService : IInitializable
    {
        public DependencyStatus  DependencyStatus  { get; private set; }
        public FirebaseAuth      FirebaseAuth      { get; private set; }
        public FirebaseUser      FirebaseUser      { get; private set; }
        public DatabaseReference DatabaseReference { get; private set; }

        public async void Initialize()
        {
            await this.CheckAndFixDependenciesAsync();

            if (this.DependencyStatus == DependencyStatus.Available)
            {
                this.FirebaseAuth = FirebaseAuth.DefaultInstance;
                this.DatabaseReference = FirebaseDatabase.GetInstance("https://resgn-of-arrow-default-rtdb.firebaseio.com/").RootReference;
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
            if (this.FirebaseAuth == null)
            {
                Debug.LogError("FirebaseAuth is not initialized.");

                return;
            }

            var loginTask = this.FirebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

            try
            {
                // Await the task to complete
                var loginResult = await loginTask;

                // User is now logged in, now get the result
                this.FirebaseUser = loginResult.User;
                onLoginSuccess?.Invoke();
                Debug.LogFormat("User signed in successfully: {0} ({1})", this.FirebaseUser.DisplayName, this.FirebaseUser.Email);
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
                var registerTask = this.FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
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
                    this.FirebaseUser = registerTask.Result.User;

                    if (this.FirebaseUser != null)
                    {
                        //Create a user profile and set the username
                        var profile = new UserProfile { DisplayName = username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        var profileTask = this.FirebaseUser.UpdateUserProfileAsync(profile);
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
    }
}
#endif