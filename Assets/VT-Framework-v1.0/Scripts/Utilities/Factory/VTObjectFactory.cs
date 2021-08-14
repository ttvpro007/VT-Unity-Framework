using System;
using UnityEditor;
using UnityEngine;

namespace VT.Utilities.Factory
{
    public static class VTObjectFactory
    {
        public static GameObject CreateGameObject(string name)
        {
            GameObject go = new GameObject(name);
            return go;
        }

        public static GameObject CreateGameObject(string name, params Type[] types)
        {
            GameObject go = CreateGameObject(name);

            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(Component)) && type != typeof(Transform))
                    go.AddComponent(type);
            }

            return go;
        }

        public static T CreateGameObject<T>(string name, params Type[] types) where T : Component
        {
            return CreateGameObject(name, types).GetComponent<T>();
        }

        public static T CreateGameObject<T>(string name) where T : Component
        {
            return CreateGameObject<T>(name, typeof(T));
        }

#if UNITY_EDITOR
        public static GameObject CreateEditorGameObject(string name)
        {
            GameObject go = ObjectFactory.CreateGameObject(name);
            Undo.RegisterCompleteObjectUndo(go, "Create " + name);

            return go;
        }

        public static GameObject CreateEditorGameObject(string name, params Type[] types)
        {
            GameObject go = CreateEditorGameObject(name);

            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(Component)) && type != typeof(Transform))
                    go.AddComponent(type);
            }

            return go;
        }

        public static T CreateEditorGameObject<T>(string name, params Type[] types) where T : Component
        {
            return CreateEditorGameObject(name, types).GetComponent<T>();
        }

        public static T CreateEditorGameObject<T>(string name) where T : Component
        {
            return CreateEditorGameObject<T>(name, typeof(T));
        }

        public static ScriptableObject CreateScriptableObjectAsset(string name, string relativePath, Type type, bool saveImmediate)
        {
            string absolutePath = VT.Utilities.PathUtils.ConvertRelativeToAbsolutePath(relativePath);

            if (!System.IO.Directory.Exists(absolutePath))
            {
                System.IO.Directory.CreateDirectory(absolutePath);
            }

            if (System.IO.Directory.Exists(absolutePath))
            {
                if (!type.IsSubclassOf(typeof(ScriptableObject)))
                {
                    Debug.LogError($"{type.Name} is not a subclass of ScriptableObject");
                    return null;
                }

                string newAssetPathWithName = AssetDatabase.GenerateUniqueAssetPath($"{relativePath}/{name}.asset");

                ScriptableObject newAsset = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(newAsset, newAssetPathWithName);

                if (saveImmediate)
                {
                    AssetDatabase.SaveAssets();
                    EditorGUIUtility.PingObject(newAsset);
                }

                return newAsset;
            }

            return null;
        }

        public static T CreateScriptableObjectAsset<T>(string name, string relativePath, bool saveImmediate) where T : ScriptableObject
        {
            return (T)CreateScriptableObjectAsset(name, relativePath, typeof(T), saveImmediate);
        }

        public static GameObject CreateSceneCanvas()
        {
            GameObject canvas = CreateEditorGameObject("Canvas", typeof(UnityEngine.UI.GraphicRaycaster), typeof(UnityEngine.UI.CanvasScaler));

            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            UnityEngine.UI.CanvasScaler cs = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
            cs.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

            return canvas;
        }
#endif
    }
}