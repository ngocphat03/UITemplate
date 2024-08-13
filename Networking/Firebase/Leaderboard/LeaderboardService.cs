#if FIREBASE && LEADERBOARD
namespace UITemplate.Leaderboard
{
    using System;
    using Zenject;
    using Firebase;
    using System.Linq;
    using UnityEngine;
    using Firebase.Auth;
    using Firebase.Database;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using AXitUnityTemplate.Networking.Firebase.Authentication;
    using AXitUnityTemplate.Networking.Firebase.Leaderboard.Data;

    public class LeaderboardService : IInitializable
    {
        private readonly AuthenticationService authenticationService;

        public LeaderboardService(AuthenticationService authenticationService) { this.authenticationService = authenticationService; }

        public FirebaseAuth      FirebaseAuth      { get; private set; }
        public FirebaseUser      FirebaseUser      { get; private set; }
        public DatabaseReference DatabaseReference { get; private set; }

        private List<DataLeaderboard> dataLeaderboards;

        public void Initialize() { }

        public void UpdateInfo()
        {
            this.FirebaseAuth = this.authenticationService.Auth;
            this.FirebaseUser = this.authenticationService.User;
            this.DatabaseReference = this.authenticationService.DatabaseReference;

            this.dataLeaderboards = new List<DataLeaderboard>();
        }

        public async UniTaskVoid SaveLeaderboardData(DataLeaderboard dataLeaderboard)
        {
            try
            {
                var json = JsonUtility.ToJson(dataLeaderboard);
                await this.DatabaseReference
                          .Child("leaderboards")
                          .Child(this.FirebaseUser.UserId)
                          .SetRawJsonValueAsync(json);
                Debug.Log("Leaderboard data saved successfully.");
            }
            catch (FirebaseException ex)
            {
                Debug.LogWarning($"Failed to save leaderboard data with {ex}");
            }
        }

        public async UniTask<DataLeaderboard> GetMyDataLeaderboard()
        {
            try
            {
                var snapshot = await this.DatabaseReference.Child("leaderboards").Child(this.FirebaseUser.UserId).GetValueAsync();
                if (snapshot.Exists)
                {
                    var dataLeaderboard = JsonUtility.FromJson<DataLeaderboard>(snapshot.GetRawJsonValue());
                    Debug.Log("Leaderboard data loaded successfully for current user.");

                    return dataLeaderboard;
                }

                Debug.LogWarning("No leaderboard data found for current user.");

                return null;
            }
            catch (FirebaseException ex)
            {
                Debug.LogWarning($"Failed to load leaderboard data with {ex}");

                return null;
            }
        }

        public async UniTask<List<DataLeaderboard>> GetAllDataLeaderboard()
        {
            try
            {
                var snapshot = await this.DatabaseReference.Child("leaderboards").GetValueAsync();
                if (snapshot.Exists)
                {
                    var listLeaderboard = snapshot.Children.Select(child => JsonUtility.FromJson<DataLeaderboard>(child.GetRawJsonValue())).ToList();
                    Debug.Log($"Leaderboard data loaded successfully for all users. Count: {listLeaderboard.Count}");

                    return listLeaderboard;
                }

                Debug.LogWarning("No leaderboard data found for any user.");

                return null;
            }
            catch (FirebaseException ex)
            {
                Debug.LogWarning($"Failed to load leaderboard data with {ex}");

                return null;
            }
        }
    }
}
#endif