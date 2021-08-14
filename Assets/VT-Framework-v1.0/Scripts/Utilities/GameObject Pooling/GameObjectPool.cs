using System;
using System.Collections.Generic;
using UnityEngine;

namespace VT.Utilities.GameObjectPooling
{
    public class GameObjectPool
    {
        private GameObject gameObject;
        private int amount;
        private Transform parent;
        private Queue<PooledGameObject> gameObjects;

        public GameObjectPool(GameObject gameObject, int amount, Transform parent)
        {
            if (!gameObject)
                throw new ArgumentException("Parameter is null", "gameObject");

            if (amount <= 0)
                throw new ArgumentException("Parameter is less or equal to 0", "gameObject");

            this.gameObject = gameObject;
            this.amount = amount;
            this.parent = parent;

            CreatePool();
        }

        private void CreatePool()
        {
            gameObjects = new Queue<PooledGameObject>();

            for (int i = 0; i < amount; i++)
            {
                GameObject cloneGameObject = UnityEngine.Object.Instantiate(gameObject, parent);
                PooledGameObject clonePooledGameObject = cloneGameObject.AddComponent<PooledGameObject>();
                gameObjects.Enqueue(clonePooledGameObject);
            }
        }

        public PooledGameObject Release()
        {
            if (gameObjects == null)
                throw new ArgumentNullException("GameObject pool has not been created");

            if (gameObjects.Count <= 0) 
                return null;

            PooledGameObject releasedGameObject = gameObjects.Dequeue();
            gameObjects.Enqueue(releasedGameObject);
            return releasedGameObject;
        }
    }
}