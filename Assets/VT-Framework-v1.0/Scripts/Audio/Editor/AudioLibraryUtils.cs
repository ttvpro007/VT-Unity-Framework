using System.IO;
using UnityEditor;
using UnityEngine;
using VT.Utilities.Factory;

namespace VT.Audio.Utils
{
    public static class AudioLibraryUtils
    {
        private const string MENU_ITEM = "Assets/Audio/Add to Audio Library %`";
        private const string AUDIO_LIBRARY_SO_RELATIVE_FOLDER_PATH = "Assets/ScriptableObjects/Audio/Resources";

        [MenuItem(MENU_ITEM, false, 100)]
        public static void AddToAudioLibrary()
        {
            AudioLibrarySO audioLibrarySO = Resources.Load<AudioLibrarySO>(typeof(AudioLibrarySO).Name);

            if (!audioLibrarySO)
            {
                audioLibrarySO = VTObjectFactory.CreateScriptableObjectAsset<AudioLibrarySO>(typeof(AudioLibrarySO).Name, AUDIO_LIBRARY_SO_RELATIVE_FOLDER_PATH, false);
            }

            if (audioLibrarySO)
            {
                Object selectedTarget = Selection.activeObject;

                if (selectedTarget && selectedTarget.GetType() == typeof(AudioClip))
                {
                    foreach (var selectedObject in Selection.objects)
                    {
                        AudioClip audioClip = (AudioClip)selectedObject;
                        AddToLibrary(audioLibrarySO, audioClip);
                    }
                }
                else
                {
                    Debug.Log("Adding to library");
                    string selectedObjectRelativePath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
                    if (Directory.Exists(Utilities.PathUtils.ConvertRelativeToAbsolutePath(selectedObjectRelativePath)))
                    {
                        Debug.Log(selectedObjectRelativePath + " is folder path");
                        string[] audioClipGUIDs = AssetDatabase.FindAssets("t:AudioClip", new string[] { selectedObjectRelativePath });
                        AudioClip[] audioClips = new AudioClip[audioClipGUIDs.Length];
                        for (int i = 0; i < audioClipGUIDs.Length; i++)
                        {
                            string audioClipPath = AssetDatabase.GUIDToAssetPath(audioClipGUIDs[i]);
                            audioClips[i] = AssetDatabase.LoadAssetAtPath<AudioClip>(audioClipPath);
                        }

                        Undo.RecordObject(audioLibrarySO, "Add tracks to Audio Library");

                        foreach (AudioClip audioClip in audioClips)
                        {
                            AddToLibrary(audioLibrarySO, audioClip);
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

        private static void AddToLibrary(AudioLibrarySO audioLibrarySO, AudioClip audioClip)
        {
            string parentFolderName = Utilities.PathUtils.GetObjectParentFolderName(audioClip);
            string clipName = Utilities.Utils.GetNiceName(audioClip.name);
            AudioProfile details = new AudioProfile(clipName, Enums.AudioType.All, audioClip);
            if (System.Enum.TryParse(parentFolderName, out Enums.AudioType audioType))
            {
                details.Type = audioType;
            }
            audioLibrarySO.Add(details);
        }
    }
}