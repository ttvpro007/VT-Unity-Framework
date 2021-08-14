using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VT.Utilities.Singleton;

namespace VT.Audio
{
    public class AudioLibrary : Singleton<AudioLibrary>
    {
        public AudioLibrarySO AudioLibrarySO
        {
            get 
            {
                if (!audioLibrarySO)
                    audioLibrarySO = Resources.Load<AudioLibrarySO>(typeof(AudioLibrarySO).Name);

                return audioLibrarySO;
            }
        }

        [SerializeField] private AudioLibrarySO audioLibrarySO;

        protected override void Awake()
        {
            base.Awake();

            audioLibrarySO = Resources.Load<AudioLibrarySO>(typeof(AudioLibrarySO).Name);
        }

        public AudioProfile GetAudioProfile(Enums.AudioTrack audioTrack)
        {
            return GetAudioProfile(audioTrack.ToString());
        }

        public AudioProfile GetAudioProfile(string name)
        {
            return audioLibrarySO.AudioProfiles[name];
        }

        public Dictionary<string, AudioProfile> GetAudioProfiles(Enums.AudioType audioTypeFilter = Enums.AudioType.All)
        {
            if (audioTypeFilter != Enums.AudioType.All)
            {
                return audioLibrarySO.AudioProfiles.Select(a => a.Value).Where(a => a.Type == audioTypeFilter).ToDictionary(a => a.Name, a => a);
            }
            else
            {
                return audioLibrarySO.AudioProfiles;
            }
        }
    }
}