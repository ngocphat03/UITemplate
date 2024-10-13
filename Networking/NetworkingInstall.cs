namespace AXitUnityTemplate.Networking
{
    using Zenject;

    public class NetworkingInstall : Installer<NetworkingInstall>
    {
        public override void InstallBindings()
        {
#if PHOTON
            AXitUnityTemplate.Networking.Photon.PhotonInstall.Install(this.Container);
#endif
            
            
        }
    }
}