namespace VT.CodeGenerator.Class
{
    public class GenericScriptableObjectClassGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Create Generic ScriptableObject File";
        private const string RELATIVE_FOLDER_PATH = "Assets/Auto Generated Code/Class";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            ClassGenerator.Generate
            (
                "VT.CodeGenerator.Class",
                new System.Type[] { typeof(UnityEngine.ScriptableObject) },
                "GenericScriptableObject",
                string.Empty,
                RELATIVE_FOLDER_PATH
            );
        }
    }
}