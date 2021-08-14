using UnityEngine;
using VT.Interfaces;

namespace VT.Utilities
{
    public abstract class Adjuster : MonoBehaviour, IAdjustable
    {
        public abstract void Increase();
        public abstract void Decrease();
    }
}
