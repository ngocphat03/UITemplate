namespace AXitUnityTemplate.UserData
{
    using System;
    using Zenject;
    using ModestTree;
    using AXitUnityTemplate.Utilities;
    using AXitUnityTemplate.UserData.Interfaces;
    using AXitUnityTemplate.UserData.UserDataManager;

    public class UserDataInstall : Installer<UserDataInstall>
    {
        public override void InstallBindings()
        {
            this.Container.DeclareSignal<UserDataLoadedSignal>();
            this.Container.Bind<IHandleUserDataServices>().To<HandleLocalUserDataServices>().AsCached();
            ReflectionUtils.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var data = Activator.CreateInstance(type);
                if (type.DerivesFrom<IUITemplateLocalData>())
                {
                    if ((data as IUITemplateLocalData)?.ControllerType is { } controllerType)
                    {
                        this.Container.Bind(type).FromInstance(data).WhenInjectedInto(controllerType);
                    }
                    else
                    {
                        this.Container.Bind(type).FromInstance(data).AsCached();
                    }
                }
                else
                {
                    this.Container.Bind(type).FromInstance(data).AsCached();
                }
            });

            this.Container.Bind<UserDataManager.UserDataManager>().AsCached();
        }
    }
}