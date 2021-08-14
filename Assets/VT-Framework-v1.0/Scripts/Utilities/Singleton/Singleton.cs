using UnityEngine;

namespace VT.Utilities.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        #region PUBLIC
        public static T Instance 
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<T>();

                return instance;
            }
        }
        #endregion

        #region PROTECTED

        protected virtual void Awake()
        {
            if (instance && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
        #endregion

        #region PRIVATE
        private static T instance;
        #endregion
    }
}