namespace AXitUnityTemplate.Networking
{
    using AXitUnityTemplate.Networking.Photon;
    using Zenject;

    public class NetworkingInstall : Installer<NetworkingInstall>
    {
        public override void InstallBindings()
        {
#if PHOTON
            PhotonInstall.Install(this.Container);
#endif
            
            
        }
    }
}