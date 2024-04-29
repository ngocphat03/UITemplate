namespace UITemplate.Scripts.Extension.Sound
{
    using System;
    using Cysharp.Threading.Tasks;
    using System.Collections.Generic;
    using UITemplate.Scripts.Extension.ObjectPool;
    using UnityEngine;
    using Zenject;

    public class SoundManager : IInitializable, IDisposable
    {
        // Inject variable
        [Inject] public IGameAssets GameAssets;

        // Private variable
        private AudioSource rootAudioSource;
        private float       volumeSound;
        private float       volumeMusic;

        // Private readonly variable
        private readonly Dictionary<string, AudioSource> musics = new();
        private readonly List<AudioSource>               sounds = new();

        // Const variable
        private const int    CountAudioCreate = 4;
        private const string KeySoundVolume   = "SoundVolume";
        private const string KeyMusicVolume   = "MusicVolume";

        public void Initialize()
        {
            // Initialize volume
            this.volumeSound = PlayerPrefs.GetFloat(SoundManager.KeySoundVolume, 1);
            this.volumeMusic = PlayerPrefs.GetFloat(SoundManager.KeyMusicVolume, 1);
            
            CreateAudioPool();

            void CreateAudioPool()
            {
                var audioObject = new GameObject { transform = { position = Vector3.zero, rotation = Quaternion.identity }, name = $"AudioSource" };
                audioObject.AddComponent<AudioSource>();
                this.rootAudioSource = audioObject.GetComponent<AudioSource>();
                this.rootAudioSource.CreatePool(SoundManager.CountAudioCreate);
            }
        }

        public async void PlaySound(string nameSound)
        {
            var audioClip = await this.GameAssets.LoadAssetAsync<AudioClip>(nameSound);
            var sound = this.SetAndPlayAudio(audioClip);
            this.sounds.Add(sound);
        }

        public async void PlayMusic(string nameMusic)
        {
            // If music has been played, stop and play again
            if (this.musics.ContainsKey(nameMusic) && this.musics[nameMusic].isPlaying)
            {
                this.musics[nameMusic].Stop();
                this.musics[nameMusic].time = 0;
                this.musics[nameMusic].Play();

                return;
            }

            var audioClip = await this.GameAssets.LoadAssetAsync<AudioClip>(nameMusic);
            var music     = this.SetAndPlayAudio(audioClip, true);
            this.musics.Add(nameMusic, music);
        }

        private AudioSource SetAndPlayAudio(AudioClip audioClip, bool loop = false)
        {
            var audio = this.rootAudioSource.Spawn();
            audio.loop = loop;
            audio.clip = audioClip;
            audio.Play();

            if (!loop) UniTask.Delay(TimeSpan.FromSeconds(audio.clip.length)).ContinueWith(() =>
            {
                audio.Recycle();
                this.sounds.Remove(audio);
            
            }).Forget();

            return audio;
        }

        public void StopMusic(string nameMusic) { this.musics[nameMusic].Stop(); }

        public void ChangeVolumeSound(float volume)
        {
            this.rootAudioSource.volume = this.volumeSound = volume;
            foreach (var sound in this.sounds) sound.volume = volume;
        }
        
        public void ChangeVolumeMusic(float volume)
        {
            this.volumeMusic = volume;
            foreach (var music in this.musics.Values) music.volume = volume;
        }

        public void Dispose()
        {
            PlayerPrefs.SetFloat(SoundManager.KeySoundVolume, this.volumeSound);
            PlayerPrefs.SetFloat(SoundManager.KeyMusicVolume, this.volumeMusic);
        }
    }
}