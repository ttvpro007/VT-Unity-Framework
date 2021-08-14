namespace VT.Audio
{
    [System.Serializable]
    public struct AudioProfile
    {
        public AudioProfile(string name, Enums.AudioType type, UnityEngine.AudioClip clip)
        {
            Name = name;
            Clip = clip;
            Type = type;
        }

        [UnityEngine.HideInInspector]
        public string Name;
        [Sirenix.OdinInspector.LabelWidth(60)]
        public UnityEngine.AudioClip Clip;
        [Sirenix.OdinInspector.LabelWidth(60)]
        public Enums.AudioType Type;
    }
}