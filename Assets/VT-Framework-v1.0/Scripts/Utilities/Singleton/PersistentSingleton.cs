namespace VT.Utilities.Singleton
{
    public class PersistentSingleton<T> : Singleton<PersistentSingleton<T>>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}