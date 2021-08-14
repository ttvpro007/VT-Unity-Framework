using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace VT.Audio
{
    public abstract class AudioPlayer : MonoBehaviour
    {
        public AudioSource AudioSource => audioSource;
        public bool IsPlaying => isPlaying;

        public float Volume
        {
            get
            {
                return volume;
            }
        }

        [Header("Audio Settings")]
        [SerializeField] protected bool mute;
        [Range(0f, 1f)]
        [SerializeField] protected float volume;
        [SerializeField] protected bool playOnStart;
        [SerializeField] protected AudioProfile currentAudioProfile;

        protected AudioSource audioSource;
        protected Dictionary<string, AudioProfile> audioProfiles;
        protected bool isPlaying;

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Start()
        {
            if (playOnStart)
                Play();
        }

        protected virtual void Initialize()
        {
            audioSource = gameObject.GetComponent<AudioSource>();

            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.mute = mute;
            audioSource.playOnAwake = false;
            audioSource.volume = volume;
            InitAudioProfiles();
            isPlaying = false;
        }

        protected abstract void InitAudioProfiles();

        public virtual void SetVolume(float volume)
        {
            this.volume = volume;

            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.volume = volume;
        }

        [Button]
        public abstract void Play();

        [Button]
        public virtual void Pause()
        {
            if (audioSource)
            {
                audioSource.Pause();
                isPlaying = false;
            }
        }

        [Button]
        public virtual void Stop()
        {
            if (audioSource)
            {
                audioSource.Stop();
                isPlaying = false;
            }
        }
    }
}