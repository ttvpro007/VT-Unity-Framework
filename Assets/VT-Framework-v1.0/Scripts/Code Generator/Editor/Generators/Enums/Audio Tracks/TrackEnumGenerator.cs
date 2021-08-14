using System.Collections.Generic;
using System.Linq;
using VT.Extensions;

namespace VT.CodeGenerator.Enums
{
    public static class TrackEnumGenerator
    {
        private const string RELATIVE_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Audio/Enums";
        private const string AUDIO_LIBRARY_SO_FILE_PATH = "Assets/VT-Framework-v1.0/Scripts/Audio/Resources/AudioLibrarySO.asset";

        public static void Generate(Audio.Enums.AudioType audioTypeFilter)
        {
            string content = "";
            Audio.AudioLibrarySO audioLibrarySO = UnityEditor.AssetDatabase.LoadAssetAtPath<Audio.AudioLibrarySO>(AUDIO_LIBRARY_SO_FILE_PATH);

            IEnumerable<Audio.AudioProfile> filteredAudioProfiles = audioLibrarySO.AudioProfiles.Values;

            if (audioTypeFilter != Audio.Enums.AudioType.All)
            {
                filteredAudioProfiles = filteredAudioProfiles.Where(a => a.Type == audioTypeFilter);
            }

            int i = 0;
            int profileCount = filteredAudioProfiles.Count();
            foreach (var filteredAudioProfile in filteredAudioProfiles)
            {
                string lineEnd = (i < profileCount - 1 ? $",{Utilities.Utils.LineBreak(1)}" : "");
                content += $"{Utilities.Utils.Tab(i != 0 ? 2 : 0)}{filteredAudioProfile.Name}" + lineEnd;
                i++;
            }

            string typeName = audioTypeFilter != Audio.Enums.AudioType.All ? audioTypeFilter.ToString() : "Audio";
            EnumGenerator.Generate("VT.Audio.Enums", $"{typeName}Track", content, RELATIVE_FOLDER_PATH);
        }
    }
}