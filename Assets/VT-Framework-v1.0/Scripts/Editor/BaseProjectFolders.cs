using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class BaseProjectFolders : EditorWindow
{
    private string windowTitle = "Base Project Folders";
    private string rootName = "Input Your New Game Name";
    private static string assetPath = string.Empty;
    private static BaseProjectFolders window = null;
    private static List<string> mainFolders = new List<string> { "Art", "Code", "Resources", "Prefabs", "Scenes" };
    private static List<string> artSubFolders = new List<string> { "Animation", "Audio", "Fonts", "Materials", "Objects", "Textures" };
    private static List<string> codeSubFolders = new List<string> { "Editor", "Scripts", "Shaders" };
    private static List<string> resourcesSubFolders = new List<string> { "Characters", "Managers", "Props", "UI" };
    private static List<string> prefabsSubFolders = new List<string> { "Characters", "Props", "UI" };
    private static List<string> scenesSubFolders = new List<string> { "Development", "Final" };
    private static Dictionary<string, List<string>> directories = null;

    /// <summary>
    /// Rendering GUI events
    /// </summary>
    private void OnGUI()
    {
        CreateTextField(ref rootName, "Project Name", true, "Desired Text Focus Control");

        AddSpace(3);

        if (GUILayout.Button("Create Folders", GUILayout.ExpandWidth(true), GUILayout.Height(30)) || Event.current.keyCode == KeyCode.Return)
        {
            CreateProjectFolders();
        }

        if (window)
            window.Repaint();
    }

    /// <summary>
    /// Create new base project folders editor window through menu item
    /// </summary>
    [MenuItem("Framework/Project Tools/Create Base Project Folders")]
    public static void CreateNewBaseProjectFoldersWindow()
    {
        window = GetWindow<BaseProjectFolders>(true, "Base Project Folders", true);
        window.maxSize = new Vector2(300, 100);
        window.minSize = window.maxSize;
        Init();
        window.Show();
    }

    /// <summary>
    /// Initialize necessary parameters
    /// </summary>
    private static void Init()
    {
        assetPath = Application.dataPath;
        directories = new Dictionary<string, List<string>>();

        for (int i = 0; i < mainFolders.Count; i++)
        {
            switch (mainFolders[i])
            {
                case "Art":         directories.Add(mainFolders[i], artSubFolders);         break;
                case "Code":        directories.Add(mainFolders[i], codeSubFolders);        break;
                case "Resources":   directories.Add(mainFolders[i], resourcesSubFolders);   break;
                case "Prefabs":     directories.Add(mainFolders[i], prefabsSubFolders);     break;
                case "Scenes":      directories.Add(mainFolders[i], scenesSubFolders);      break;
            }
        }
    }

    /// <summary>
    /// Create essential project folders
    /// </summary>
    private void CreateProjectFolders()
    {
        if (string.IsNullOrEmpty(rootName.Trim()))
        {
            DisplayWarningDialogue(windowTitle, "Folder's name cannot be left blank!");
            return;
        }

        string path = string.Empty;

        foreach (string folder in directories.Keys)
        {
            foreach (string subFolder in directories[folder])
            {
                path = assetPath + "/" + rootName + "/" + folder + "/" + subFolder;

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
            }
        }

        CreateBasicScenes();

        AssetDatabase.Refresh();

        if (window)
            window.Close();
    }

    /// <summary>
    /// 1/ Create basic scenes needed for the project...
    /// 2/ Add the final scenes to build settings...
    /// </summary>
    private void CreateBasicScenes()
    {
        string path = string.Empty;

        foreach (string folder in scenesSubFolders)
        {
            switch (folder)
            {
                case "Development":
                    path = "Assets" + "/" + rootName + "/Scenes/Development/" + rootName + "_Development.unity";
                    CreateAndSaveScene(path);
                    break;
                case "Final":
                    List<EditorBuildSettingsScene> finalSceneList = new List<EditorBuildSettingsScene>();

                    path = "Assets" + "/" + rootName + "/Scenes/Final/" + rootName + "_StartUp.unity";
                    CreateAndSaveScene(path);
                    finalSceneList.Add(new EditorBuildSettingsScene(path, true));

                    path = "Assets" + "/" + rootName + "/Scenes/Final/" + rootName + "_FrontEnd.unity";
                    CreateAndSaveScene(path);
                    finalSceneList.Add(new EditorBuildSettingsScene(path, true));

                    path = "Assets" + "/" + rootName + "/Scenes/Final/" + rootName + "_Main.unity";
                    CreateAndSaveScene(path);
                    finalSceneList.Add(new EditorBuildSettingsScene(path, true));

                    EditorBuildSettings.scenes = finalSceneList.ToArray();
                    break;
            }
        }
    }

    // TODO Move to helper classes
    #region Helpers
    /// <summary>
    /// Create and save a new scene
    /// </summary>
    /// <param name="path"> The destination path to save the new scene </param>
    private static void CreateAndSaveScene(string path)
    {
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        EditorSceneManager.SaveScene(newScene, path);
    }

    /// <summary>
    /// Add space between elements
    /// </summary>
    /// <param name="amount"> The amount of new space needed </param>
    private static void AddSpace(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EditorGUILayout.Space();
        }
    }

    /// <summary>
    /// Create a text field
    /// </summary>
    /// <param name="textField"> Reference to the string that holds the value of the text field </param>
    /// <param name="label"> Label of the text field </param>
    /// <param name="focus"> Set to be focused or not </param>
    /// <param name="nextControlName"> Name for focus control </param>
    private static void CreateTextField(ref string textField, string label, bool focus, string nextControlName = "")
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label + ": ", EditorStyles.boldLabel, GUILayout.MaxWidth(100f));

        if (focus)
        {
            // Set focus control
            GUI.SetNextControlName(nextControlName);
            textField = EditorGUILayout.TextField(textField);
            GUI.FocusControl(nextControlName);
        }
        else
        {
            textField = EditorGUILayout.TextField(textField);
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Create a popup warning dialogue
    /// </summary>
    /// <param name="message"></param>
    private static void DisplayWarningDialogue(string title, string message)
    {
        EditorUtility.DisplayDialog(title + " Warning!", message, "OK");
    }
    #endregion
}