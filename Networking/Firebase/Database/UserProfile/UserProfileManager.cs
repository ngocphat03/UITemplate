namespace AXitUnityTemplate.Networking.Firebase.Database.UserProfile
{
    using System;
    using UnityEngine;
    using global::Firebase;
    using global::Firebase.Auth;
    using System.Threading.Tasks;
    using global::Firebase.Database;
    using System.Collections.Generic;
    using System.Linq;
    using AXitUnityTemplate.Networking.Firebase.Authentication;
    using AXitUnityTemplate.Networking.Firebase.Database.UserProfile.Data;
    using AXitUnityTemplate.Scripts.Extension;
    using Newtonsoft.Json;
    using Zenject;

    public class UserProfileManager : IInitializable
    {
        private readonly AuthenticationService authenticationService;
        private readonly FirebaseAuth          firebaseAuth;
        private          DatabaseReference     databaseReference;

        public UserProfileManager(AuthenticationService authenticationService) { this.authenticationService = authenticationService; }

        public FirebaseAuth FirebaseAuth { get; private set; }
        public FirebaseUser FirebaseUser { get; private set; }

        public async Task AddUserProfile(User user)
        {
            return;
            try
            {
                var json = JsonConvert.SerializeObject(user);
                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("User profile data is empty.");

                    return;
                }

                await this.databaseReference
                          .Child(DatabasePaths.Users)
                          .Child(this.FirebaseUser.UserId)
                          .SetRawJsonValueAsync(json);
                Debug.Log("User profile add successfully.");
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"Failed to add user profile with {ex}");
            }
        }

        public async Task<User> LoadUserProfile(string userId)
        {
            try
            {
                var snapshot = await this.databaseReference
                                         .Child(DatabasePaths.Users)
                                         .Child(userId)
                                         .GetValueAsync();
                if (!snapshot.Exists)
                {
                    Debug.LogError("No user profile data found.");

                    return null;
                }

                var user = this.Deserialize<User>(snapshot);
                Debug.Log("User profile loaded successfully.");

                return user;
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"Failed to load user profile with {ex}");

                return null;
            }
        }

        public async Task AddItemToInventory(Item item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("Item data is empty.");

                    return;
                }

                await this.databaseReference
                          .Child(DatabasePaths.Users)
                          .Child(this.FirebaseUser.UserId)
                          .Child(DatabasePaths.Inventory)
                          .Child(item.ItemId)
                          .SetRawJsonValueAsync(json);
                Debug.Log("Item added to inventory successfully.");
            }
            catch (FirebaseException ex)
            {
                Debug.LogError($"Failed to add item to inventory with {ex}");
            }
        }

        public async Task AddFriend(string userId, Friend friend)
        {
            try
            {
                var json = JsonConvert.SerializeObject(friend);
                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogError("Item data is empty.");

                    return;
                }

                await this.databaseReference
                          .Child(DatabasePaths.Users)
                          .Child(userId)
                          .Child(DatabasePaths.Friends)
                          .Child(friend.FriendId)
                          .SetRawJsonValueAsync(json);
                Debug.Log("Friend added successfully.");
            }
            catch (FirebaseException ex)
            {
                Debug.LogWarning($"Failed to add friend with {ex}");
            }
        }

        public async Task<List<Friend>> GetFriendsList(string userId)
        {
            try
            {
                var snapshot = await this.databaseReference
                                         .Child(DatabasePaths.Users)
                                         .Child(userId)
                                         .Child(DatabasePaths.Friends)
                                         .GetValueAsync();
                if (!snapshot.Exists)
                {
                    Debug.LogWarning("No friends found.");

                    return null;
                }

                var friendsList = snapshot.Children
                                          .Select(this.Deserialize<Friend>)
                                          .Where(friend => friend != null)
                                          .ToList();
                Debug.Log("Friends list loaded successfully.");

                return friendsList;
            }
            catch (FirebaseException ex)
            {
                Debug.LogWarning($"Failed to load friends list with {ex}");

                return null;
            }
        }

        public void UpdateInfo()
        {
            this.FirebaseAuth      = this.authenticationService.Auth;
            this.FirebaseUser      = this.authenticationService.User;
            this.databaseReference = this.authenticationService.DatabaseReference;
        }

        public async void Initialize()
        {
            // this.UpdateInfo();
            // var userProfile = await this.LoadUserProfile("phatdeptrai");
        }

        private T Deserialize<T>(DataSnapshot snapshot) => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(snapshot.Value));
    }
}