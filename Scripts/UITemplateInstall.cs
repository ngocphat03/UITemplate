namespace UITemplate.Scripts
{
#if PHOTON
    using UITemplate.Photon.Scripts;
#endif
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UITemplate.Scripts.Extension.StateMachine.Signal;
    using UITemplate.Scripts.Managers;
    using UnityEngine;
    using Zenject;

    public class UITemplateInstall : Installer<UITemplateInstall>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(this.Container);
            this.InstallSignal();
            this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached();
            this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy();
            this.Container.Bind<IScreenManager>().FromInstance(Object.FindObjectOfType<ScreenManager>()).AsSingle().NonLazy();

#if PHOTON
            this.Container.BindInterfacesAndSelfTo<UITemplatePhotonService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
#endif
        }

        private void InstallSignal()
        {
            //State machine
            this.Container.DeclareSignal<OnStateEnterSignal>();
            this.Container.DeclareSignal<OnStateExitSignal>();
        }
    }
}