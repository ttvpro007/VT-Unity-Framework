using VT.Extensions;

namespace VT.CodeGenerator.Enums
{
    public static class SceneInBuildEnumGenerator
    {
        private const string ITEM_NAME = "VT Framework/Code Generator/Create Scene In Build Enum File";
        private const string RELATIVE_FOLDER_PATH = "Assets/Auto Generated Code/Enums";

        [UnityEditor.MenuItem(ITEM_NAME)]
        public static void Generate()
        {
            string content = string.Empty;

            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension
                                    (
                                        UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)
                                    )
                                    .Replace(" ", string.Empty);
                content += (i > 0 ? Utilities.Utils.Tab(2) : "") + $"{sceneName} = {i}" + (i < sceneCount - 1 ? "," + Utilities.Utils.LineBreak(1) : "");
            }

            string projectNamespace = Utilities.PathUtils.GetProjectName().ConvertToNamespaceQualifiedName();
            EnumGenerator.Generate($"{projectNamespace}.Enums", "SceneInBuild", content, RELATIVE_FOLDER_PATH);
        }
    }
}