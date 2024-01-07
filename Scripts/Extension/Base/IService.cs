namespace UITemplate.Scripts.Extension.Base
{
    using Cysharp.Threading.Tasks;

    public interface IService
    {
        public virtual UniTask Init() { return UniTask.CompletedTask;}
    }
}