namespace AXitUnityTemplate.UserData.UserDataManager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AXitUnityTemplate.UserData;
    using AXitUnityTemplate.UserData.Interfaces;
    using AXitUnityTemplate.Utilities;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Scripting;
    using Zenject;

    public class UserDataManager
    {
        private readonly DiContainer    container;
        private readonly SignalBus               signalBus;
        private readonly IHandleUserDataServices handleUserDataService;
        
        public UserDataManager(DiContainer container, SignalBus signalBus, IHandleUserDataServices handleUserDataService)
        {
            this.container             = container;
            this.signalBus             = signalBus;
            this.handleUserDataService = handleUserDataService;
        }

        public async UniTask LoadUserData()
        {
            await UniTask.NextFrame();
            var types     = ReflectionUtils.GetAllDerivedTypes<ILocalData>().ToArray();
            var datas     = await this.handleUserDataService.Load(types);
            var dataCache = (Dictionary<string, ILocalData>)typeof(BaseHandleUserDataServices).GetField("userDataCache", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this.handleUserDataService);
            IterTools.Zip(types, datas).ForEach((type, data) =>
            {
                var boundData = (data as IUITemplateLocalData)?.ControllerType is { } controllerType
                    ? controllerType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .First(fieldInfo => fieldInfo.FieldType == type)
                        .GetValue(this.container.Resolve(controllerType))
                    : this.container.Resolve(type);

                data.CopyTo(boundData);
                dataCache[BaseHandleUserDataServices.KeyOf(type)] = (ILocalData)boundData;
            });
            this.signalBus.Fire<UserDataLoadedSignal>();
        }
    }
}