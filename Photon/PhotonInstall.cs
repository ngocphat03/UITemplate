﻿#if PHOTON
namespace UITemplate.Photon
{
    using UITemplate.Photon.Scripts;
    using UITemplate.Photon.Signals;
    using Zenject;

    public class PhotonInstall : Installer<PhotonInstall>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<UITemplatePhotonService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

            this.Container.DeclareSignal<OnCreateRoomSignal>();
            this.Container.DeclareSignal<OnUpdateRoomSignal>();
        }
    }
}
#endif