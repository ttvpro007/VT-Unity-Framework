namespace VT.CodeGenerator.Enums
{
    public static class AudioTrackEnumGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Audio/Create Audio Track Enum File";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            TrackEnumGenerator.Generate(Audio.Enums.AudioType.All);
        }
    }
}