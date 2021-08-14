using UnityEngine;

namespace VT.Utilities.GameObjectPooling
{
    [CreateAssetMenu(fileName = "New GameObject Pool Settings SO", menuName = "VT/Utilities/Object Pooling/Create GameObject Pool Settings SO")]
    public class GameObjectPoolSettingsSO : ScriptableObject
    {
        public GameObject GameObject;
        public int Amount;
    }
}