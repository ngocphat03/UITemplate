namespace AXitUnityTemplate.Blueprint.BlueprintControlFlow
{
    using Zenject;
    using AXitUnityTemplate.Utilities;
    using AXitUnityTemplate.Blueprint.Signals;
    using AXitUnityTemplate.Blueprint.BlueprintReader;

    /// <summary>
    /// Binding all services of the blueprint control flow at here
    /// </summary>
    public class BlueprintServicesInstaller : Installer<BlueprintServicesInstaller>
    {
        public override void InstallBindings()
        {
            //BindBlueprint reader for mobile
            this.Container.Bind<PreProcessBlueprintMobile>().AsCached().NonLazy();
            this.Container.Bind<BlueprintReaderManager>().AsCached();

            this.Container.BindAllDerivedTypes<IGenericBlueprintReader>();
            this.Container.Bind<BlueprintConfig>().AsCached();


            this.Container.DeclareSignal<LoadBlueprintDataSucceedSignal>();
            this.Container.DeclareSignal<LoadBlueprintDataProgressSignal>();
            this.Container.DeclareSignal<ReadBlueprintProgressSignal>();
        }
    }
}