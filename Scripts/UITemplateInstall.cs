namespace UITemplate.Scripts
{
    using AXitUnityTemplate.Networking.FirebaseNetwork;
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
            this.InstallOther();
        }

        private void InstallSignal()
        {
            //State machine
            this.Container.DeclareSignal<OnStateEnterSignal>();
            this.Container.DeclareSignal<OnStateExitSignal>();
        }

        private void InstallOther()
        {
#if PHOTON
#endif
            
#if ADS_SERVICE
            UITemplate.AdsService.AdsServiceInstall.Install(this.Container);
#endif

#if FIREBASE && LEADERBOARD
            Leaderboard.LeaderboardInstall.Install(this.Container);
            FirebaseInstall.Install(this.Container);
#endif
        }
    }
}