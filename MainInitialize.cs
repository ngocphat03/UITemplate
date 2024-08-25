namespace UITemplate
{
    using System.Collections.Generic;
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


    public class DataModel
    {
        public List<NormalPerson> NormalPersons;
    }

    public class NormalPerson
    {
        public People People { get; set; }
        public List<Animal> Animals { get; set; }
    }

    public class People
    {
        public string Name   { get; set; }
        public int    Age    { get; set; }
        public string Gender { get; set; }
    }
    
    public class Animal
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}