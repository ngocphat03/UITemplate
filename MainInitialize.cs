namespace UITemplate
{
    using System.Collections.Generic;
    using UITemplate.Scripts.Extension.Base;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UITemplate.Scripts.Extension.Sound;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class MainInitialize : MonoBehaviour
    {
        private readonly List<MonoService> listServiceHadCreate = new List<MonoService>();
        
        private async void Awake()
        {
            // add service
            this.listServiceHadCreate.Add(CreateService<ObjectPoolManager>());
            this.listServiceHadCreate.Add(CreateService<SoundManager>());
            
            foreach (var monoService in this.listServiceHadCreate)
            {
                await monoService.Init();
            }
            
            MonoService CreateService<T>() where T : MonoBehaviour
            {
                var newService = new GameObject { transform = { position = Vector3.zero, rotation = Quaternion.identity }, name = typeof(T).Name };
                newService.AddComponent<T>();
                Object.DontDestroyOnLoad(newService);
                return newService.GetComponent<T>() as MonoService;
            }
        }
    }
}