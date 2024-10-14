namespace AXitUnityTemplate.Blueprint.BlueprintControlFlow
{
    using System;
    using Zenject;
    using System.IO;
    using System.Linq;
    using UnityEngine;
    using System.IO.Compression;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using AXitUnityTemplate.Utilities;
    using AXitUnityTemplate.Blueprint.Signals;
    using AXitUnityTemplate.Blueprint.BlueprintReader;

    /// <summary>
    ///  The main manager for reading blueprints pipeline/>.
    /// </summary>
    public class BlueprintReaderManager
    {
        #region zeject

        private readonly SignalBus           signalBus;
        private readonly DiContainer         diContainer;
        private readonly BlueprintConfig     blueprintConfig;

        #endregion

        private readonly ReadBlueprintProgressSignal readBlueprintProgressSignal = new();

        public BlueprintReaderManager(SignalBus          signalBus,          DiContainer         diContainer, BlueprintConfig blueprintConfig)
        {
            this.signalBus           = signalBus;
            this.diContainer         = diContainer;
            this.blueprintConfig     = blueprintConfig;
        }

        public virtual async UniTask LoadBlueprint()
        {
            Dictionary<string, string> listRawBlueprints = null;

            listRawBlueprints = new Dictionary<string, string>();
            this.signalBus.Fire(new LoadBlueprintDataProgressSignal { Percent = 1f });

            //Load all blueprints to instances
            try
            {
                await this.ReadAllBlueprint(listRawBlueprints);
            }
            catch (Exception e)
            {
                throw new Exception($"[BlueprintReader] Error while reading blueprint: {e.Message}");
            }
            
            Debug.Log("[BlueprintReader] All blueprint are loaded");
            this.signalBus.Fire<LoadBlueprintDataSucceedSignal>();
        }

        protected virtual async UniTask<Dictionary<string, string>> UnzipBlueprint()
        {
            var result = new Dictionary<string, string>();
            if (!File.Exists(this.blueprintConfig.BlueprintZipFilepath))
            {
                return result;
            }

            using var archive = ZipFile.OpenRead(this.blueprintConfig.BlueprintZipFilepath);
            foreach (var entry in archive.Entries)
            {
                if (!entry.FullName.EndsWith(this.blueprintConfig.BlueprintFileType, StringComparison.OrdinalIgnoreCase))
                    continue;
                using var streamReader   = new StreamReader(entry.Open());
                var       readToEndAsync = await streamReader.ReadToEndAsync();
                result.Add(entry.Name, readToEndAsync);
            }

            return result;
        }

        private UniTask ReadAllBlueprint(Dictionary<string, string> listRawBlueprints)
        {
            if (!File.Exists(this.blueprintConfig.BlueprintZipFilepath))
            {
                Debug.LogWarning($"[BlueprintReader] {this.blueprintConfig.BlueprintZipFilepath} is not exists!!!, Continue load from resource");
            }

            var listReadTask    = new List<UniTask>();
            var allDerivedTypes = ReflectionUtils.GetAllDerivedTypes<IGenericBlueprintReader>();
            this.readBlueprintProgressSignal.MaxBlueprint    = allDerivedTypes.Count();
            this.readBlueprintProgressSignal.CurrentProgress = 0;
            this.signalBus.Fire(this.readBlueprintProgressSignal); // Inform that we just start reading blueprint
            foreach (var blueprintType in allDerivedTypes)
            {
                var blueprintInstance = (IGenericBlueprintReader)this.diContainer.Resolve(blueprintType);
                if (blueprintInstance != null)
                {
#if !UNITY_WEBGL
                    listReadTask.Add(UniTask.RunOnThreadPool(() => this.OpenReadBlueprint(blueprintInstance, listRawBlueprints)));
#else
                    listReadTask.Add(UniTask.Create(() => this.OpenReadBlueprint(blueprintInstance, listRawBlueprints)));
#endif
                }
                else
                {
                    Debug.Log($"Can not resolve blueprint {blueprintType.Name}");
                }
            }

            return UniTask.WhenAll(listReadTask);
        }

        private async UniTask OpenReadBlueprint(IGenericBlueprintReader blueprintReader, Dictionary<string, string> listRawBlueprints)
        {
            var bpAttribute = blueprintReader.GetCustomAttribute<BlueprintReaderAttribute>();
            if (bpAttribute != null)
            {
                if (bpAttribute.BlueprintScope == BlueprintScope.Server) return;

                // Try to load a raw blueprint file from local or resource folder
                string rawCsv;
                if (this.blueprintConfig.IsResourceMode || bpAttribute.IsLoadFromResource)
                {
                    rawCsv = await LoadRawCsvFromResourceFolder();
                }
                else
                {
                    if (!listRawBlueprints.TryGetValue(bpAttribute.DataPath + this.blueprintConfig.BlueprintFileType, out rawCsv))
                    {
                        Debug.LogWarning($"[BlueprintReader] Blueprint {bpAttribute.DataPath} is not exists at the local folder, try to load from resource folder");
                        rawCsv = await LoadRawCsvFromResourceFolder();
                    }
                }

                async UniTask<string> LoadRawCsvFromResourceFolder()
                {
                    await UniTask.SwitchToMainThread();
                    var result = string.Empty;
                    try
                    {
                        result = ((TextAsset)await Resources.LoadAsync<TextAsset>(this.blueprintConfig.ResourceBlueprintPath + bpAttribute.DataPath)).text;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Load {bpAttribute.DataPath} blueprint error!!!: {e.Message}");
                    }

#if !UNITY_WEBGL
                    await UniTask.SwitchToThreadPool();
#endif
                    return result;
                }

                // Deserialize the raw blueprint to the blueprint reader instance

                if (!string.IsNullOrEmpty(rawCsv))
                {
                    await blueprintReader.DeserializeFromCsv(rawCsv);
                    lock (this.readBlueprintProgressSignal)
                    {
                        this.readBlueprintProgressSignal.CurrentProgress++;
                        this.signalBus.Fire(this.readBlueprintProgressSignal);
                    }
                }
                else
                    Debug.LogWarning($"[BlueprintReader] Unable to load {bpAttribute.DataPath} from {(bpAttribute.IsLoadFromResource ? "resource folder" : "local folder")}!!!");
            }
            else
            {
                Debug.LogWarning($"[BlueprintReader] Class {blueprintReader} does not have BlueprintReaderAttribute yet");
            }
        }
    }
}