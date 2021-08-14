
namespace VT.CodeGenerator
{
    public static class CodeGeneratorUtils
    {
        public static void ImportAssetIntoUnity(string relativeFilePath, bool ping)
        {
            UnityEditor.AssetDatabase.ImportAsset(relativeFilePath);

            if (ping)
            {
                UnityEditor.EditorGUIUtility.PingObject(UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(relativeFilePath));
            }
        }

        public static string MassReplace(this string source, (string, string)[] replacePairs)
        {
            foreach (var replacePair in replacePairs)
            {
                source = source.Replace(replacePair.Item1, replacePair.Item2);
            }

            return source;
        }

        public static void CreateTemplateTextFile(string content, string filePath)
        {
            UnityEngine.Debug.Log($"{filePath} does not exist!");
            CreateAndImportFile(content, filePath, false);
            UnityEngine.Debug.Log($"Template file created.");
        }

        public static void CreateDirectoryIfNotExist(string folderPath)
        {
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
        }

        public static void CreateAndImportTextFile(string fileName, string content, string folderPath, bool ping)
        {
            CreateAndImportFile(fileName, "txt", content, folderPath, ping);
        }

        public static void CreateAndImportCSFile(string fileName, string content, string folderPath, bool ping)
        {
            CreateAndImportFile(fileName, "cs", content, folderPath, ping);
        }

        public static void CreateAndImportFile(string fileName, string fileExtension, string content, string folderPath, bool ping)
        {
            string filePath = $"{folderPath}/{fileName}.{fileExtension}";
            CreateAndImportFile(content, filePath, ping);
        }

        public static void CreateAndImportFile(string content, string filePath, bool ping)
        {
            string folderPath = System.IO.Path.GetDirectoryName(filePath);

            CreateDirectoryIfNotExist(folderPath);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.Write(content);
            }

            ImportAssetIntoUnity(Utilities.PathUtils.ConvertAbsoluteToRelativePath(filePath), ping);
        }

        public static string ReadFromFile(string filePath)
        {
            string content = string.Empty;

            if (System.IO.File.Exists(filePath))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
            }

            return content;
        }
    }
}