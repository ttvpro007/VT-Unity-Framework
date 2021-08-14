using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace VT.SceneManagement
{
    public static class SceneLoader
    {
        #region PUBLIC
        public static int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        public static string CurrentSceneName => SceneManager.GetActiveScene().name;

        public static event UnityAction<Scene, LoadSceneMode> SceneLoaded
        {
            add
            {
                lock (objectLock)
                {
                    SceneManager.sceneLoaded += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    SceneManager.sceneLoaded -= value;
                }
            }
        }

        public static bool ValidateScene(int sceneIndex)
        {
            bool valid = sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings;
            if (!valid) Debug.LogWarning("[SceneLoader]: Invalid scene index!");
            return valid;
        }

        public static bool ValidateScene(string sceneName)
        {
            bool valid = Application.CanStreamedLevelBeLoaded(sceneName);
            if (!valid) Debug.LogWarning("[SceneLoader]: Invalid scene name!");
            return valid;
        }

        public static void LoadScene(string sceneName)
        {
            if (ValidateScene(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public static void LoadScene(int sceneIndex)
        {
            if (ValidateScene(sceneIndex))
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }

        public static void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadNextScene()
        {
            LoadScene(GetNextSceneIndex());
        }

        public static SceneLoaderMonoBehaviour LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!ValidateScene(sceneName)) return null;

            SceneLoaderMonoBehaviour sceneLoaderMonoBehaviour = SceneLoaderMonoBehaviour.Create();
            sceneLoaderMonoBehaviour.LoadSceneAsync(sceneName, mode);
            return sceneLoaderMonoBehaviour;
        }

        public static SceneLoaderMonoBehaviour LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!ValidateScene(sceneIndex)) return null;
            
            SceneLoaderMonoBehaviour sceneLoaderMonoBehaviour = SceneLoaderMonoBehaviour.Create();
            sceneLoaderMonoBehaviour.LoadSceneAsync(sceneIndex, mode);
            return sceneLoaderMonoBehaviour;
        }

        public static SceneLoaderMonoBehaviour UnloadSceneAsync(string sceneName)
        {
            if (!ValidateScene(sceneName)) return null;

            SceneLoaderMonoBehaviour sceneLoaderMonoBehaviour = SceneLoaderMonoBehaviour.Create();
            sceneLoaderMonoBehaviour.UnloadSceneAsync(sceneName);
            return sceneLoaderMonoBehaviour;
        }

        public static SceneLoaderMonoBehaviour UnloadSceneAsync(int sceneIndex)
        {
            if (!ValidateScene(sceneIndex)) return null;

            SceneLoaderMonoBehaviour sceneLoaderMonoBehaviour = SceneLoaderMonoBehaviour.Create();
            sceneLoaderMonoBehaviour.UnloadSceneAsync(sceneIndex);
            return sceneLoaderMonoBehaviour;
        }

        public static void ReloadSceneAsync()
        {
            LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public static void LoadNextSceneAsync()
        {
            LoadSceneAsync(GetNextSceneIndex());
        }
        #endregion

        #region PRIVATE
        private static object objectLock = new object();

        private static int GetNextSceneIndex()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            return nextSceneIndex < SceneManager.sceneCountInBuildSettings ? nextSceneIndex : nextSceneIndex - 1;
        }
        #endregion
    }
}