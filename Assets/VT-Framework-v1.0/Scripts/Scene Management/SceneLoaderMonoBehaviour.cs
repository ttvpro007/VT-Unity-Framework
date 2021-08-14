using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VT.SceneManagement
{
    public class SceneLoaderMonoBehaviour : MonoBehaviour
    {
        #region PUBLIC
        public AsyncOperation AsyncOperation => asyncOperation;

        public static SceneLoaderMonoBehaviour Create()
        {
            GameObject gameObject = new GameObject("SceneLoader-MonoBehaviour");
            return gameObject.AddComponent<SceneLoaderMonoBehaviour>();
        }

        public void LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            StartCoroutine(LoadSceneAsyncRoutine(sceneName, mode));
        }

        public void LoadSceneAsync(int sceneIndex, LoadSceneMode mode)
        {
            StartCoroutine(LoadSceneAsyncRoutine(sceneIndex, mode));
        }

        public void UnloadSceneAsync(string sceneName)
        {
            StartCoroutine(UnloadSceneAsyncRoutine(sceneName));
        }

        public void UnloadSceneAsync(int sceneIndex)
        {
            StartCoroutine(UnloadSceneAsyncRoutine(sceneIndex));
        }
        #endregion

        #region PRIVATE
        private AsyncOperation asyncOperation;

        private IEnumerator LoadSceneAsyncRoutine(string sceneName, LoadSceneMode mode)
        {
            yield return RunSceneAsyncRoutine(SceneManager.LoadSceneAsync(sceneName, mode));
        }

        private IEnumerator LoadSceneAsyncRoutine(int sceneIndex, LoadSceneMode mode)
        {
            yield return RunSceneAsyncRoutine(SceneManager.LoadSceneAsync(sceneIndex, mode));
        }

        private IEnumerator UnloadSceneAsyncRoutine(string sceneName)
        {
            yield return RunSceneAsyncRoutine(SceneManager.UnloadSceneAsync(sceneName));
        }

        private IEnumerator UnloadSceneAsyncRoutine(int sceneIndex)
        {
            yield return RunSceneAsyncRoutine(SceneManager.UnloadSceneAsync(sceneIndex));
        }

        private IEnumerator RunSceneAsyncRoutine(AsyncOperation asyncOperation)
        {
            this.asyncOperation = asyncOperation;

            while (!this.asyncOperation.isDone)
            {
                yield return null;
            }

            Destroy(gameObject);
        }
        #endregion
    }
}