using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T instance;
    public static T Instance { get { return instance; } }

    #region UNITY ENGINE FUNCTION

    protected virtual void Awake()
    {
        if (instance)
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
}
