using UnityEngine;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    public class PooledGameObjectSpawner : MonoBehaviour
    {
        public PooledGameObject SpawnPooledGameObjectAt(Vector3 spawnPostion)
        {
            PooledGameObject pooledGameObject = GameObjectPoolManager.Instance.GetGameObjectFromPool(gameObjectPool.ToString());

            if (!pooledGameObject) return null;

            pooledGameObject.transform.position = spawnPostion;
            pooledGameObject.SetActive();
            return pooledGameObject;
        }

        [SerializeField] protected PoolType gameObjectPool;
    }
}