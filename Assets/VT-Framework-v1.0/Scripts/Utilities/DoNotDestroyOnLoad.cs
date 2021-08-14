using UnityEngine;

namespace VT.Utilities
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}