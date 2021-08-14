using VT.Extensions;

namespace VT.CodeGenerator.Enums
{
    public static class SortingLayerEnumGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Create Sorting Layer Enum File";
        private const string RELATIVE_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Utilities/Enums";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            string content = string.Empty;

            int sortingLayerCount = UnityEngine.SortingLayer.layers.Length;
            for (int i = 0; i < sortingLayerCount; i++)
            {
                UnityEngine.SortingLayer sortingLayer = UnityEngine.SortingLayer.layers[i];
                content += (i > 0 ? Utilities.Utils.Tab(2) : "") + $"{sortingLayer.name} = {sortingLayer.id}" + (i < sortingLayerCount - 1 ? "," + Utilities.Utils.LineBreak(1) : "");
            }

            string projectNamespace = Utilities.PathUtils.GetProjectName().ConvertToNamespaceQualifiedName();
            EnumGenerator.Generate($"{projectNamespace}.Enums", "SortingLayer", content, RELATIVE_FOLDER_PATH);
        }
    }
}