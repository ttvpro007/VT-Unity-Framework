namespace VT.CodeGenerator.Enums
{
    public static class DNPPrefabsEnumGenerator
    {
        private const string RELATIVE_FOLDER_PATH = "Assets/VT-Framework-v1.0/DamageNumbersPro/Auto Generated Code/Enums";
        private const string DNP_PREFABS_DETAILS_SO_FILE_PATH = "Assets/VT-Framework-v1.0/DamageNumbersPro/ScriptableObjects/Resources/DNPPrefabsLibrarySO.asset";
        private const string ITEM_NAME = "VT Framework/Code Generator/Create DNP Prefabs Enum File";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            string content = "";
            DamageNumbersPro.DNPPrefabsLibrarySO dnpPrefabsLibrarySO = UnityEditor.AssetDatabase.LoadAssetAtPath<DamageNumbersPro.DNPPrefabsLibrarySO>(DNP_PREFABS_DETAILS_SO_FILE_PATH);

            int i = 0;
            int detailCount = dnpPrefabsLibrarySO.DNPPrefabDetails.Count;
            foreach (var dnpPrefabName in dnpPrefabsLibrarySO.DNPPrefabDetails.Keys)
            {
                string lineEnd = (i < detailCount - 1 ? $",{VT.Utilities.Utils.LineBreak(1)}" : "");
                content += $"{VT.Utilities.Utils.Tab(i != 0 ? 2 : 0)}{dnpPrefabName}" + lineEnd;
                i++;
            }

            EnumGenerator.Generate("DamageNumbersPro.Enums", "DNPPrefabs", content, RELATIVE_FOLDER_PATH);
        }
    }
}