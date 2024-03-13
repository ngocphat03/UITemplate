namespace UITemplate.Photon.Scripts
{
    using Cysharp.Threading.Tasks;
    using global::Photon.Pun;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.Base;
    using UnityEngine;

    public class UITemplatePhotonService : MonoService
    {
        private readonly IGameAssets gameAssets = ObjectFactoryExtension.GetService<GameAssets>();

        private PhotonController photonController;
        public override UniTask Init()
        {
            Debug.Log($"Photon status: {PhotonNetwork.NetworkClientState.ToString()}");

            if (this.photonController == null)
            {
                var newPhotonController = new GameObject(nameof(PhotonController)).AddComponent<PhotonController>();
                newPhotonController.transform.SetParent(this.transform);
                this.photonController = newPhotonController;
            }

            return UniTask.CompletedTask;
        }

        public void CreateRoom(string nameRoom) { }

        public void JoinRoom(string nameRoom) { }

        public bool CheckHasRoom(string nameRoom) { return false; }

        public void LeaveRoom() { }
    }
}