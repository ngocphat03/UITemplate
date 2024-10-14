namespace AXitUnityTemplate.GameAssets.Utilities
{
    using System;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using Object = UnityEngine.Object;
    using UnityEngine.AddressableAssets;
    using UnityEngine.AddressableAssets.ResourceLocators;
    using UnityEngine.ResourceManagement.AsyncOperations;
    
    public class LoadAssetsByLabelOperation : AsyncOperationBase<List<AsyncOperationHandle<Object>>>
    {
        private readonly string label;
        private readonly Dictionary<object, AsyncOperationHandle> loadedDictionary;
        private readonly Dictionary<object, AsyncOperationHandle> loadingDictionary;

        public LoadAssetsByLabelOperation(Dictionary<object, AsyncOperationHandle> loadedDictionary, Dictionary<object, AsyncOperationHandle> loadingDictionary,
            string label)
        {
            this.loadedDictionary  = loadedDictionary ?? new Dictionary<object, AsyncOperationHandle>();
            this.loadingDictionary = loadingDictionary ?? new Dictionary<object, AsyncOperationHandle>();
            this.label             = label;
        }

        protected override void Execute()
        {
            #pragma warning disable CS4014
            this.DoTask();
            #pragma warning restore CS4014
        }

        public async UniTask DoTask()
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(this.label);
            var locations       = await locationsHandle.Task;

            var loadingInternalIdDic = new Dictionary<string, AsyncOperationHandle<Object>>();
            var loadedInternalIdDic = new Dictionary<string, AsyncOperationHandle<Object>>();

            var operationHandles = new List<AsyncOperationHandle<Object>>();
            foreach (var resourceLocation in locations)
            {
                AsyncOperationHandle<Object> loadingHandle = Addressables.LoadAssetAsync<Object>(resourceLocation.PrimaryKey);

                operationHandles.Add(loadingHandle);

                loadingInternalIdDic.TryAdd(resourceLocation.InternalId, loadingHandle);

                loadingHandle.Completed += assetOp =>
                {
                    loadedInternalIdDic.TryAdd(resourceLocation.InternalId, assetOp);
                };
            }
            
            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    bool isGuid = Guid.TryParse(key.ToString(), out _);
                    if (!isGuid)
                        continue;
                    
                    if (!this.TryGetKeyLocationID(locator, key, out var keyLocationID))
                        continue;

                    var locationMatched = loadingInternalIdDic.TryGetValue(keyLocationID, out var loadingHandle);
                    if (!locationMatched)
                        continue;

                    this.loadingDictionary.TryAdd(key, loadingHandle);
                }
            }

            foreach (var handle in operationHandles)
                await handle.Task;

            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    bool isGuid = Guid.TryParse(key.ToString(), out _);
                    if (!isGuid)
                        continue;
                    
                    if (!this.TryGetKeyLocationID(locator, key, out var keyLocationID))
                        continue;

                    var locationMatched = loadedInternalIdDic.TryGetValue(keyLocationID, out var loadedHandle);
                    if (!locationMatched)
                        continue;

                    if (this.loadingDictionary.ContainsKey(key))
                        this.loadingDictionary.Remove(key);
                    this.loadedDictionary.TryAdd(key, loadedHandle);
                }
            }

            this.Complete(operationHandles, true, string.Empty);
        }

        private bool TryGetKeyLocationID(IResourceLocator locator, object key, out string internalID)
        {
            internalID = string.Empty;
            var hasLocation = locator.Locate(key, typeof(Object), out var keyLocations);
            if (!hasLocation)
                return false;
            switch (keyLocations.Count)
            {
                case 0:
                case > 1:
                    return false;
                default:
                    internalID = keyLocations[0].InternalId;
                    return true;
            }
        }
    }
}