namespace VT.CodeGenerator.Enums
{
    public static class MusicTrackEnumGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Audio/Create Music Track Enum File";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            TrackEnumGenerator.Generate(Audio.Enums.AudioType.Music);
        }
    }
}