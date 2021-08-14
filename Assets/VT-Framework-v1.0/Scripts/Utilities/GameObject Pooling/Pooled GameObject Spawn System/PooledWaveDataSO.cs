using Sirenix.OdinInspector;
using UnityEngine;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    [CreateAssetMenu(fileName = "New Pooled Wave Data SO", menuName = "VT/Spawn System/Create Pooled Wave Data SO")]
    public class PooledWaveDataSO : ScriptableObject
    {
        #region PUBLIC
        public PoolType GameObjectPool => gameObjectPool;
        public int SpawnAmount => spawnAmount;
        public int SpawnLimit => spawnLimit;
        public float SpawnInterval => spawnInterval;
        #endregion

        #region PROTECTED
        [SerializeField] protected PoolType gameObjectPool;
        [SerializeField, MinValue(0)] protected int spawnAmount;
        [SerializeField, MinValue(0)] protected int spawnLimit;
        [SerializeField, MinValue(0)] protected float spawnInterval;
        #endregion
    }
}