using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;
using VT.Audio.Enums;

namespace VT.Audio
{
    public class SFXPlayer : AudioPlayer
    {
        [Button]
        public void Play(SFXTrack sfxTrack, int repeats = 1)
        {
            Play(sfxTrack.ToString(), repeats);
        }

        public void PlayRandom(params SFXTrack[] sfxTracks)
        {
            Play(sfxTracks[UnityEngine.Random.Range(0, sfxTracks.Length)]);
        }

        public void PlayMix(params SFXTrack[] sfxTracks)
        {
            foreach (var sfxTrack in sfxTracks)
            {
                Play(sfxTrack);
            }
        }

        public override void Play()
        {
            if (audioSource)
            {
                audioSource.PlayOneShot(currentAudioProfile.Clip);
            }
        }

        public void Play(string sfxTrackName, int repeats = 1)
        {
            currentAudioProfile = AudioLibrary.Instance.GetAudioProfile(sfxTrackName);

            if (repeats > 1)
            {
                PlayLoops(repeats);
            }
            else
            {
                Play();
            }
        }

        private void PlayLoops(int repeats)
        {
            Play();

            Sequence playSequence = DOTween.Sequence();

            for (int i = 1; i < repeats; i++)
            {
                playSequence.Append(DOVirtual.DelayedCall(currentAudioProfile.Clip.length, Play));
            }

            playSequence.Play();
        }

        protected override void InitAudioProfiles()
        {
            audioProfiles = AudioLibrary.Instance.GetAudioProfiles(AudioType.SFX);
        }
    }
}