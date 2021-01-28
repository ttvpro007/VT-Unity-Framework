using System;
using System.Collections.Generic;
using UnityEngine;

namespace VT.Utilities
{
    public class RuntimeMonoBehaviour
    {
        private class MonoBehaviourHook : MonoBehaviour
        {
            public Action OnUpdated = default;
            
            private void Update()
            {
                OnUpdated?.Invoke();
            }
        }

        private static Dictionary<string, RuntimeMonoBehaviour> runtimeMonoBehaviourDictionary;
        private static GameObject globalGameObject;

        public static RuntimeMonoBehaviour Create(string name, Action updateDelegate)
        {
            return Create(name, () => { updateDelegate?.Invoke(); return false; });
        }
        public static RuntimeMonoBehaviour Create(string name, Func<bool> updateDelegate)
        {
            Initialize();

            GameObject gameObject = new GameObject("RuntimeMonoBehaviour - " + name, typeof(MonoBehaviourHook));
            gameObject.transform.parent = globalGameObject.transform;

            RuntimeMonoBehaviour runtimeMonoBehaviour = new RuntimeMonoBehaviour(gameObject, name, updateDelegate);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdated = runtimeMonoBehaviour.Update;

            runtimeMonoBehaviourDictionary.Add(name, runtimeMonoBehaviour);

            return runtimeMonoBehaviour;
        }

        private static void Initialize()
        {
            if (!globalGameObject)
            {
                globalGameObject = new GameObject("RuntimeMonoBehaviour - Global");
                runtimeMonoBehaviourDictionary = new Dictionary<string, RuntimeMonoBehaviour>();
            }
        }

        private static bool RemoveRuntimeMonoBehaviour(string name)
        {
            return runtimeMonoBehaviourDictionary.Remove(name);
        }

        private GameObject gameObject;
        private Func<bool> updateDelegate;
        private string name;
        private bool isActive;

        public RuntimeMonoBehaviour(GameObject gameObject, string name, Func<bool> updateDelegate)
        {
            this.gameObject = gameObject;
            this.updateDelegate = updateDelegate;
            this.name = name;
            isActive = true;
        }

        public RuntimeMonoBehaviour SetActive(bool value)
        {
            isActive = value;
            return this;
        }

        private void Update()
        {
            if (!isActive) return;
            if (updateDelegate?.Invoke() == true)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            RemoveRuntimeMonoBehaviour(name);

            if (gameObject)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }
}