using System.IO;
using UnityEditor;
using UnityEngine;
using VT.Utilities.Factory;

namespace DamageNumbersPro
{
    public static class DNPPrefabsLibraryUtils
    {
        private const string MENU_ITEM = "Assets/Damage Number Pro/Add to DNP Prefab Library";
        private const string AUDIO_LIBRARY_SO_RELATIVE_FOLDER_PATH = "Assets/VT-Framework-v1.0/DamageNumbersPro/Scriptable Objects/Resources";

        [MenuItem(MENU_ITEM, false, 100)]
        public static void AddToAudioLibrary()
        {
            DNPPrefabsLibrarySO dnpPrefabsLibrarySO = Resources.Load<DNPPrefabsLibrarySO>(typeof(DNPPrefabsLibrarySO).Name);
            Debug.Log("dnpPrefabsLibrarySO is null: " + dnpPrefabsLibrarySO == null);

            if (!dnpPrefabsLibrarySO)
            {
                dnpPrefabsLibrarySO = VTObjectFactory.CreateScriptableObjectAsset<DNPPrefabsLibrarySO>(typeof(DNPPrefabsLibrarySO).Name, AUDIO_LIBRARY_SO_RELATIVE_FOLDER_PATH, false);
            }

            if (dnpPrefabsLibrarySO)
            {
                Object selectedTarget = Selection.activeObject;

                if (selectedTarget && selectedTarget.GetType() == typeof(GameObject))
                {
                    foreach (var selectedObject in Selection.objects)
                    {
                        GameObject prefab = (GameObject)selectedObject;
                        if (prefab.GetComponent<DamageNumber>())
                        {
                            AddToLibrary(dnpPrefabsLibrarySO, prefab);
                        }
                    }
                }
                else
                {
                    Debug.Log("Adding to library");
                    string selectedObjectRelativePath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
                    if (Directory.Exists(VT.Utilities.PathUtils.ConvertRelativeToAbsolutePath(selectedObjectRelativePath)))
                    {
                        Debug.Log(selectedObjectRelativePath + " is folder path");
                        string[] gameObjectGUIDs = AssetDatabase.FindAssets("t:GameObject", new string[] { selectedObjectRelativePath });
                        GameObject[] gameObjects = new GameObject[gameObjectGUIDs.Length];
                        for (int i = 0; i < gameObjectGUIDs.Length; i++)
                        {
                            string gameObjectPath = AssetDatabase.GUIDToAssetPath(gameObjectGUIDs[i]);
                            gameObjects[i] = AssetDatabase.LoadAssetAtPath<GameObject>(gameObjectPath);
                        }

                        Undo.RecordObject(dnpPrefabsLibrarySO, $"Add prefabs to DNP Prefab Library");

                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (gameObject.GetComponent<DamageNumber>())
                            {
                                AddToLibrary(dnpPrefabsLibrarySO, gameObject);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log(selectedObjectRelativePath + " is NOT folder path");
                    }
                }

                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();
            }
        }

        private static void AddToLibrary(DNPPrefabsLibrarySO dnpPrefabsLibrarySO, GameObject dnpPrefab)
        {
            string prefabName = VT.Utilities.Utils.GetNiceName(dnpPrefab.name);
            dnpPrefabsLibrarySO.Add(prefabName, dnpPrefab);
        }
    }
}