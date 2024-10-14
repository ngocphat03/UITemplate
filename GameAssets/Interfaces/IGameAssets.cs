namespace AXitUnityTemplate.GameAssets.Interfaces
{
    using UnityEngine;
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public interface IGameAssets
    {
        AsyncOperationHandle DownloadDependenciesAsync(AssetLabelReference labelReference);
        AsyncOperationHandle DownloadDependenciesAsync(IEnumerable keys, Addressables.MergeMode mode = Addressables.MergeMode.Intersection);
        /// <summary>
        /// Load scene in Addressable by key
        /// </summary>
        /// <param name="key">The key of the location of the scene to load.</param>
        /// <param name="loadMode"><see cref="LoadSceneMode"/></param>
        /// <param name="activeOnLoad">If false, the scene will load but not activate (for background loading).  The SceneInstance returned has an Activate() method that can be called to do this at a later point.</param>
        AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = LoadSceneMode.Single, bool activeOnLoad = true);
        /// <summary>
        /// Load scene in Addressable by AssetReference
        /// </summary>
        AsyncOperationHandle<SceneInstance> LoadSceneAsync(AssetReference sceneRef, LoadSceneMode loadMode = LoadSceneMode.Single, bool activeOnLoad = true);
        /// <summary>
        /// Release scene by key
        /// </summary>
        /// <param name="key">The key of the location of the scene to unload.</param>
        AsyncOperationHandle<SceneInstance> UnloadSceneAsync(object key);
        /// <summary>
        /// Release scene by AssetReference
        /// </summary>
        AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AssetReference sceneRef);
        /// <summary>
        /// Unload all auto unload assets in scene
        /// </summary>
        /// <param name="sceneName"> Scene Target</param>
        void UnloadUnusedAssets(string sceneName);
        /// <summary>
        ///     Preload assets for target scene
        /// </summary>
        /// <param name="targetScene">default is current scene</param>
        /// <param name="keys"></param>
        /// <returns></returns>
        List<AsyncOperationHandle<T>> PreloadAsync<T>(string targetScene = "", params object[] keys);
        AsyncOperationHandle<List<AsyncOperationHandle<Object>>> LoadAssetsByLabelAsync(string label);
        /// <summary>
        /// Load a single asset by key
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="key">The key of the location of the asset.</param>
        /// <param name="isAutoUnload">If true, asset will be automatically released when the current scene was unloaded</param>
        /// <param name="targetScene">scene that asset will be released when it unloaded if isAutoUnload = true</param>
        AsyncOperationHandle<T> LoadAssetAsync<T>(object key, bool isAutoUnload = true, string targetScene = "");
        /// <summary>
        /// Load a single asset by AssetReference
        /// </summary>
        AsyncOperationHandle<T> LoadAssetAsync<T>(AssetReference assetReference, bool isAutoUnload = true);
        /// <summary>
        /// Load a single asset synchronously
        /// Warning:  a method called WaitForCompletion() that force the async operation to complete and return the Result of the operation. May have performance implications on runtime
        /// </summary>
        /// <param name="key">The key of the location of the asset.</param>
        /// <param name="isAutoUnload">If true, asset will be automatically released when the current scene was unloaded</param>
        /// <typeparam name="T">The type of the asset.</typeparam>
        T ForceLoadAsset<T>(object key, bool isAutoUnload = true);
        /// <summary>
        /// Release asset and its associated resources by key
        /// </summary>
        /// <param name="key">The key of the location of the asset to release.</param>
        void ReleaseAsset(object key);
        /// <summary>
        /// Release asset and its associated resources by AssetReference
        /// </summary>
        void ReleaseAsset(AssetReference assetReference);
        /// <summary>
        /// Instantiate async a GameObject by key
        /// </summary>
        UniTask<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true);

        public bool DestroyGameObject(GameObject gameObject);
        
        Dictionary<object, AsyncOperationHandle> GetLoadingAssets();
    }
}