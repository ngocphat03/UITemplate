namespace UITemplate.Scripts.Extension.Base
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class MonoService : MonoBehaviour, IService
    {
        public virtual UniTask Init() { return UniTask.CompletedTask;}
    }
}