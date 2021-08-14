using UnityEngine;
using VT.Interfaces;
using VT.Utilities.Singleton;

namespace VT.Audio
{
    public class AudioManager : Singleton<AudioManager>, ISavable
    {
        public MusicPlayer MusicPlayer => musicPlayer;
        public SFXPlayer SFXPlayer => sfxPlayer;
        public float MusicVolume => musicVolume;
        public float SFXVolume => sfxVolume;

        [SerializeField, Range(0, 1)] private float musicVolume;
        [SerializeField, Range(0, 1)] private float sfxVolume;
        [SerializeField] private MusicPlayer musicPlayer;
        [SerializeField] private SFXPlayer sfxPlayer;

        private void Start()
        {
            Load();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = value;
            musicPlayer.SetVolume(musicVolume);
        }

        public void SetSFXVolume(float value)
        {
            sfxVolume = value;
            sfxPlayer.SetVolume(sfxVolume);
        }

        public void Save()
        {
            ES3.Save("Music Volume", musicVolume);
            ES3.Save("SFX Volume", sfxVolume);
        }

        public void Load()
        {
            SetMusicVolume(ES3.Load("Music Volume", 1f));
            SetSFXVolume(ES3.Load("SFX Volume", 1f));
        }
    }
}