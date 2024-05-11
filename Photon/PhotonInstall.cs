#if PHOTON
namespace UITemplate.Photon
{
    using UITemplate.Photon.Data;
    using UITemplate.Photon.Scripts;
    using UITemplate.Photon.Signals;
    using UnityEngine;
    using Zenject;

    public class PhotonInstall : Installer<PhotonInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UITemplatePhotonRoomService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<UITemplatePhotonTurnController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            this.Container.BindInterfacesAndSelfTo<DataRealTimeManager>().AsSingle().NonLazy();
            
            this.Container.DeclareSignal<OnCreateRoomSignal>();
            this.Container.DeclareSignal<OnUpdateRoomSignal>();
            this.Container.DeclareSignal<StartOnlineGameSignal>();
            this.Container.DeclareSignal<InYourTurnSignal>();
            this.Container.DeclareSignal<NotifyForAllPlayerSignal>();
            this.Container.DeclareSignal<OnJoinRoomSignal>();
        }
    }
}
#endif
