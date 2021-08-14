using System.Collections.Generic;
using UnityEngine;

namespace VT.Audio
{
    [CreateAssetMenu(fileName = "New Audio Library SO", menuName = "VT/Audio/Create Audio Library SO")]
    public class AudioLibrarySO : Sirenix.OdinInspector.SerializedScriptableObject
    {
        public Dictionary<string, AudioProfile> AudioProfiles
        {
            get
            {
                if (audioProfiles == null)
                    audioProfiles = new Dictionary<string, AudioProfile>();

                return audioProfiles;
            }
        }

        public void Add(AudioProfile details)
        {
            if (!audioProfiles.ContainsKey(details.Name))
            {
                audioProfiles.Add(details.Name, details);
            }
            else if (audioProfiles[details.Name].Clip != details.Clip)
            {
                int count = 1;
                string name = details.Name + count;
                while (audioProfiles.ContainsKey(name))
                {
                    name = details.Name + ++count;

                    if (count > audioProfiles.Count)
                        break;
                }
                details.Name = name;
                audioProfiles.Add(details.Name, details);
            }
        }

        private void OnEnable()
        {
            if (audioProfiles == null)
                audioProfiles = new Dictionary<string, AudioProfile>();
        }

        [Sirenix.OdinInspector.DictionaryDrawerSettings(KeyLabel = "Name", ValueLabel = "Details"), SerializeField]
        private Dictionary<string, AudioProfile> audioProfiles = new Dictionary<string, AudioProfile>();
    }
}