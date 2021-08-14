using System;
using System.Collections.Generic;
using UnityEngine;
using VT.Extensions;

namespace VT.Utilities
{
    public class RuntimeMonoBehaviour
    {
        #region PUBLIC
        public MonoBehaviour MonoBehaviour => monoBehaviour;

        public void SetMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }

        public static RuntimeMonoBehaviour Create(string name, Action updateDelegate)
        {
            return Create(name, () => { updateDelegate?.Invoke(); return false; });
        }

        public static RuntimeMonoBehaviour Create(string name, Func<bool> updateDelegate)
        {
            if (!isInitialized)
                Initialize();

            int dateTimeHash = DateTime.Now.GetHashCode();
            string hashedName = name + dateTimeHash;

            GameObject gameObject = new GameObject($"RuntimeMonoBehaviour - {hashedName}", typeof(MonoBehaviourHook));
            gameObject.transform.parent = globalGameObject.transform;

            RuntimeMonoBehaviour runtimeMonoBehaviour = new RuntimeMonoBehaviour(gameObject, name, updateDelegate);
            MonoBehaviourHook monoBehaviourHook = gameObject.GetComponent<MonoBehaviourHook>();
            monoBehaviourHook.OnUpdated = runtimeMonoBehaviour.Update;
            runtimeMonoBehaviour.SetMonoBehaviour(monoBehaviourHook);

            while (runtimeMonoBehaviourDictionary.ContainsKey(hashedName))
                hashedName = name + ++dateTimeHash;

            runtimeMonoBehaviourDictionary.Add(hashedName, runtimeMonoBehaviour);

            return runtimeMonoBehaviour;
        }

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

        public void DestroySelf()
        {
            RemoveRuntimeMonoBehaviour(name);

            if (gameObject)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        #endregion

        #region PRIVATE
        private static bool isInitialized = false;
        private static Dictionary<string, RuntimeMonoBehaviour> runtimeMonoBehaviourDictionary;
        private static GameObject globalGameObject = null;

        private GameObject gameObject;
        private Func<bool> updateDelegate;
        private string name;
        private bool isActive;
        private MonoBehaviour monoBehaviour;

        private class MonoBehaviourHook : MonoBehaviour
        {
            public Action OnUpdated = default;

            private void Update()
            {
                OnUpdated?.Invoke();
            }
        }

        private static void Initialize()
        {
            globalGameObject = globalGameObject.CreateNewIfNull("RuntimeMonoBehaviour - Global");
            runtimeMonoBehaviourDictionary = new Dictionary<string, RuntimeMonoBehaviour>();
            isInitialized = true;
        }

        private static bool RemoveRuntimeMonoBehaviour(string name)
        {
            return runtimeMonoBehaviourDictionary.Remove(name);
        }

        private void Update()
        {
            if (!isActive) return;
            if (updateDelegate?.Invoke() == true)
            {
                DestroySelf();
            }
        }
        #endregion
    }
}