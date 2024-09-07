namespace AXitUnityTemplate.Networking.FirebaseNetwork.RealtimeDatabase
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AXitUnityTemplate.Networking.FirebaseNetwork.Signals;
    using Firebase.Database;
    using UnityEngine;
    using Zenject;

    public class RealtimeDatabaseService : IInitializable, IDisposable
    {
        private readonly SignalBus         signalBus;
        private          DatabaseReference databaseReference;

        public RealtimeDatabaseService(SignalBus signalBus) { this.signalBus = signalBus; }

        public void Initialize() { this.signalBus.Subscribe<FirebaseAuthenticationInitializedSignal>(this.OnFirebaseAuthenticationInitializedSignal); }

        private void OnFirebaseAuthenticationInitializedSignal(FirebaseAuthenticationInitializedSignal signal) { this.databaseReference = signal.DatabaseReference; }

        public async Task SaveDataAsync<T>(string path, T data)
        {
            try
            {
                var jsonData = JsonUtility.ToJson(data);
                await this.databaseReference.Child(path).SetRawJsonValueAsync(jsonData);
                Debug.Log($"Data saved to path: {path}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to {path}: {e.Message}");
            }
        }

        public async Task<T> LoadDataAsync<T>(string path)
        {
            try
            {
                var snapshot = await this.databaseReference.Child(path).GetValueAsync();
                if (snapshot.Exists)
                {
                    var jsonData = snapshot.GetRawJsonValue();

                    return JsonUtility.FromJson<T>(jsonData);
                }

                Debug.LogWarning($"No data found at path: {path}");

                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data from {path}: {e.Message}");

                return default;
            }
        }

        public async Task SaveDataWithMultipleChildAsync<T>(List<string> pathList, T data)
        {
            try
            {
                var reference = this.databaseReference;

                reference = pathList.Aggregate(reference, (current, path) => current.Child(path));

                var jsonData = JsonUtility.ToJson(data);
                await reference.SetRawJsonValueAsync(jsonData);

                Debug.Log($"Data saved to path: {string.Join("/", pathList)}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to path {string.Join("/", pathList)}: {e.Message}");
            }
        }
        
        public async Task<T> LoadDataWithMultipleChildAsync<T>(List<string> pathList)
        {
            try
            {
                var reference = this.databaseReference;
                reference = pathList.Aggregate(reference, (current, path) => current.Child(path));
                var snapshot = await reference.GetValueAsync();

                if (snapshot.Exists)
                {
                    var jsonData = snapshot.GetRawJsonValue();
                    var      data     = JsonUtility.FromJson<T>(jsonData);
                    Debug.Log($"Data loaded from path: {string.Join("/", pathList)}");
                    return data;
                }

                Debug.LogWarning($"No data found at path: {string.Join("/", pathList)}");
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data from path {string.Join("/", pathList)}: {e.Message}");
                return default;
            }
        }


        public void Dispose() { this.signalBus.Unsubscribe<FirebaseAuthenticationInitializedSignal>(this.OnFirebaseAuthenticationInitializedSignal); }
    }
}