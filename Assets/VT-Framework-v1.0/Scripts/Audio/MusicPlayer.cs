using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VT.Extensions;

namespace VT.Audio
{
    public class MusicPlayer : AudioPlayer
    {
        public enum PlayType
        {
            Default,
            Shuffle,
            ShuffleLoop,
            Loop,
            LoopOne
        }

        [SerializeField] private bool fading;
        [SerializeField] private PlayType playType;

        private const float FADE_DURATION = 0.3f;
        private const float FADE_BUFFER = 0.3f;

        protected event Action onClipFinished;
        private Stack<int> nextPlayIndexStack;
        private Stack<int> previousPlayIndexStack;
        private Tween fadingTween;

        private void Update()
        {
            if (!isPlaying) return;

            if (fading)
            {
                if (IsFadeInTime())
                {
                    FadeIn();
                }
                else if (IsFadeOutTime())
                {
                    FadeOut();
                }
            }
        }

        protected override void InitAudioProfiles()
        {
            audioProfiles = AudioLibrary.Instance.GetAudioProfiles(Enums.AudioType.Music);
        }

        protected override void Initialize()
        {
            base.Initialize();

            onClipFinished += OnTrackFinishedHandler;

            nextPlayIndexStack = Enumerable.Range(0, audioProfiles.Count).Reverse().ToStack();
            previousPlayIndexStack = new Stack<int>();

            if (playType == PlayType.Shuffle || playType == PlayType.ShuffleLoop)
            {
                nextPlayIndexStack = nextPlayIndexStack.Shuffle().ToStack();
            }

            if (playType == PlayType.LoopOne)
            {
                audioSource.loop = true;
            }

            if (!currentAudioProfile.Clip)
                currentAudioProfile = GetNextAudioProfile();
        }

        private void OnTrackFinishedHandler()
        {
            switch (playType)
            {
                case PlayType.Default:
                    PlayNext();
                    break;
                case PlayType.Shuffle:
                    PlayNext();
                    break;
                case PlayType.ShuffleLoop:
                    if (nextPlayIndexStack.Count <= 0)
                        nextPlayIndexStack = Enumerable.Range(0, audioProfiles.Count - 1).Shuffle().ToStack();
                    PlayNext();
                    break;
                case PlayType.Loop:
                    if (nextPlayIndexStack.Count <= 0)
                        nextPlayIndexStack = Enumerable.Range(0, audioProfiles.Count - 1).ToStack();
                    PlayNext();
                    break;
                case PlayType.LoopOne:
                    audioSource.loop = true;
                    Play();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Play(string musicTrackName)
        {
            currentAudioProfile = audioProfiles[musicTrackName];

            Play();
        }

        public void Play(Enums.MusicTrack musicTrack)
        {
            Play(musicTrack.ToString());
        }

        public override void Play()
        {
            if (!audioSource) return;
            
            audioSource.clip = currentAudioProfile.Clip;
            audioSource.volume = volume;
            audioSource.Play();
            isPlaying = true;
        }

        [Button]
        private void PlayNext()
        {
            Stop();

            currentAudioProfile = GetNextAudioProfile();
            Play();

            if (audioSource)
                audioSource.loop = false;
        }

        [Button]
        private void PlayPrevious()
        {
            Stop();

            currentAudioProfile = GetPreviousAudioProfile();
            Play();

            if (audioSource)
                audioSource.loop = false;
        }

        private bool IsFadeInTime()
        {
            return audioSource.time <= FADE_DURATION + FADE_BUFFER;
        }

        private bool IsFadeOutTime()
        {
            return audioSource.time >= audioSource.clip.length - (FADE_DURATION + FADE_BUFFER);
        }

        private void FadeOut()
        {
            if (fadingTween == null)
            {
                fadingTween = audioSource.DOFade(0f, FADE_DURATION)
                    .OnComplete(() =>
                    {
                        onClipFinished?.Invoke();
                        fadingTween = null;
                    });
            }
        }

        private void FadeIn()
        {
            if (fadingTween == null)
            {
                fadingTween = audioSource.DOFade(volume, FADE_DURATION)
                    .OnComplete(() =>
                    {
                        fadingTween = null;
                    });
            }
        }

        private int activeTrackIndex;

        private AudioProfile GetPreviousAudioProfile()
        {
            if (previousPlayIndexStack.Count > 0)
            {
                activeTrackIndex = previousPlayIndexStack.Pop();
                nextPlayIndexStack.Push(activeTrackIndex);
            }

            return audioProfiles.ElementAt(activeTrackIndex).Value;
        }

        private AudioProfile GetNextAudioProfile()
        {
            if (nextPlayIndexStack.Count > 0)
            {
                activeTrackIndex = nextPlayIndexStack.Pop();
                previousPlayIndexStack.Push(activeTrackIndex);
            }

            return audioProfiles.ElementAt(activeTrackIndex).Value;
        }
    }
}