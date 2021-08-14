using VT.Extensions;

namespace VT.CodeGenerator.Enums
{
    public static class PoolTypeEnumGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Create Pool Type Enum File";
        private const string RELATIVE_FOLDER_PATH = "Assets/VT-Framework-v1.0/Scripts/Utilities/GameObject Pooling/Enums";
        private const string POOL_SO_FOLDER_PATH = "Assets/Game/ScriptableObjects/GameObject Pooling";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            string content = "None,\n";
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(POOL_SO_FOLDER_PATH);
            System.IO.FileInfo[] fileInfos = directoryInfo.GetFiles("*.asset");
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Name.Contains("Pool"))
                {
                    string poolName = fileInfos[i].Name.Split('.')[0].Replace(" ", string.Empty);
                    string lineEnd = (i < fileInfos.Length - 1 ? $", {Utilities.Utils.LineBreak(1)}" : "");
                    content += $"{Utilities.Utils.Tab(2)}{poolName}" + lineEnd;
                }
            }

            string projectNamespace = Utilities.PathUtils.GetProjectName().ConvertToNamespaceQualifiedName();
            EnumGenerator.Generate($"{projectNamespace}.Enums", "PoolType", content, RELATIVE_FOLDER_PATH);
        }
    }
}