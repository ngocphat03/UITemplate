namespace UITemplate.Scripts.Extension.Sound
{
    using System.Collections;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UITemplate.Scripts.Extension.Base;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UnityEngine;

    public class SoundManager : MonoService
    {
        public           List<AudioSource> listAudio        = new List<AudioSource>();
        private readonly IGameAssets       gameAssets       = ObjectFactoryExtension.GetService<GameAssets>();
        private const    int               CountAudioCreate = 4;
        public static    SoundManager      Instance;
        private          AudioSource       rootAudioSource;

        public override UniTask Init()
        {
            Instance = this;
            CreateAudioPool();

            return base.Init();

            void CreateAudioPool()
            {
                var audioObject = new GameObject { transform = { position = Vector3.zero, rotation = Quaternion.identity }, name = $"AudioSource" };
                audioObject.AddComponent<AudioSource>();
                this.rootAudioSource = audioObject.GetComponent<AudioSource>();
                this.rootAudioSource.CreatePool(CountAudioCreate);
                this.listAudio = this.rootAudioSource.GetPooled();
            }
        }

        public void PlaySound(string nameSound)
        {
            var audioClip = this.gameAssets.LoadAssetAsync<AudioClip>(nameSound).WaitForCompletion();
            this.StartCoroutine(SetAndPlayAudio(this.rootAudioSource.Spawn()));

            IEnumerator SetAndPlayAudio(AudioSource audi)
            {
                audi.clip = audioClip;
                audi.Play();

                yield return new WaitForSeconds(audi.clip.length);
                audi.Recycle();
            }
        }
    }
}