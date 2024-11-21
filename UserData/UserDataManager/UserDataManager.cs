namespace AXitUnityTemplate.UserData.UserDataManager
{
    using System.Linq;
    using System.Reflection;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using AXitUnityTemplate.UserData;
    using AXitUnityTemplate.Utilities;
    using AXitUnityTemplate.UserData.Interfaces;

#if ZENJECT
    using Zenject;
    public class UserDataManager
    {
        private readonly DiContainer    container;
        private readonly SignalBus               signalBus;
        private readonly IHandleUserDataServices handleUserDataService;
        
        public UserDataManager(DiContainer container, SignalBus signalBus, IHandleUserDataServices handleUserDataService)
        {
            this.container = container;
            this.signalBus = signalBus;
            this.handleUserDataService = handleUserDataService;
        }

        public async UniTask LoadUserData()
        {
            await UniTask.NextFrame();
            var types = ReflectionUtils.GetAllDerivedTypes<ILocalData>().ToArray();
            var datas = await this.handleUserDataService.Load(types);
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

#elif VCONTAINER
    using VContainer;
    public class UserDataManager
    {
        private readonly IObjectResolver         container;
        private readonly IHandleUserDataServices handleUserDataService;

        public UserDataManager(IObjectResolver         container,
                               IHandleUserDataServices handleUserDataService)
        {
            this.container             = container;
            this.handleUserDataService = handleUserDataService;
        }

        public async UniTask LoadUserData()
        {
            var types     = ReflectionUtils.GetAllDerivedTypes<ILocalData>().ToArray();
            var datas     = await this.handleUserDataService.Load(types);
            var dataCache = (Dictionary<string, ILocalData>)typeof(BaseHandleUserDataServices).GetField("userDataCache", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this.handleUserDataService);
            IterTools.Zip(types, datas).ForEach((type, data) =>
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var boundData = (data as IUITemplateLocalData)?.ControllerType is { } controllerType
                    ? controllerType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                    .First(fieldInfo => fieldInfo.FieldType == type)
                                    .GetValue(this.container.Resolve(controllerType))
                    : this.container.Resolve(type);

                data.CopyTo(boundData);
                dataCache[BaseHandleUserDataServices.KeyOf(type)] = (ILocalData)boundData;
            });
            // TODO: Fire signal (UserDataLoadedSignal)
        }
    }
#else
    // Mono
#endif
}