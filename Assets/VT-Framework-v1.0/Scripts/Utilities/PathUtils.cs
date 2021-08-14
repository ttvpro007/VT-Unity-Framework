using System.Linq;

namespace VT.Utilities
{
#if UNITY_EDITOR
    public static class PathUtils
    {
        private static readonly string ASSET_PATH_ROOT = "Assets";

        public static string GetProjectName()
        {
            var splitedPath = UnityEngine.Application.dataPath.Split('/');
            return splitedPath[splitedPath.Length - 2];
        }

        public static string ConvertRelativeToAbsolutePath(string relativePath)
        {
            if (IsRelativePath(relativePath))
            {
                return UnityEngine.Application.dataPath + relativePath.Substring(ASSET_PATH_ROOT.Length);
            }
            else if (IsAbsolutePath(relativePath))
            {
                return relativePath;
            }

            throw new System.ArgumentException($"{relativePath} is neither absolute or relative path.");
        }

        public static string ConvertAbsoluteToRelativePath(string absolutePath)
        {
            if (IsAbsolutePath(absolutePath))
            {
                return ASSET_PATH_ROOT + absolutePath.Substring(UnityEngine.Application.dataPath.Length);
            }
            else if (IsRelativePath(absolutePath))
            {
                return absolutePath;
            }

            throw new System.ArgumentException($"{absolutePath} is neither absolute or relative path.");
        }

        public static string GetObjectParentDirectory(UnityEngine.Object selectedObject)
        {
            return System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(selectedObject));
        }

        public static string GetObjectParentFolderName(UnityEngine.Object selectedObject)
        {
            return System.IO.Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(selectedObject)).Split('\\').Last();
        }

        public static bool IsAbsolutePath(string path)
        {
            return path.StartsWith(UnityEngine.Application.dataPath);
        }

        public static bool IsRelativePath(string path)
        {
            return path.StartsWith(ASSET_PATH_ROOT);
        }
    }
#endif
}