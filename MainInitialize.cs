namespace UITemplate
{
    using System.Collections.Generic;
    using UITemplate.Photon.Scripts;
    using UITemplate.Scripts.Extension;
    using UITemplate.Scripts.Extension.Base;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UITemplate.Scripts.Extension.Sound;
    using UnityEngine;

    public class MainInitialize : MonoBehaviour
    {
        private readonly List<MonoService> listServiceHadCreate = new List<MonoService>();

        private SafeArea safeArea;
        
        private void Awake()
        {
        }
    }
}