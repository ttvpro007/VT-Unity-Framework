using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VT.Extensions;
using VT.Utilities.Singleton;

namespace VT.Utilities.GameObjectPooling
{
    public class GameObjectPoolManager : Singleton<GameObjectPoolManager>
    {
        [SerializeField, InlineEditor]
        private List<GameObjectPoolSettingsSO> gameObjectPoolSettingsList;

        private Dictionary<string, GameObjectPool> gameObjectPools;

        protected override void Awake()
        {
            base.Awake();

            gameObjectPools = new Dictionary<string, GameObjectPool>();

            foreach (var gameObjectPoolSettingsItem in gameObjectPoolSettingsList)
            {
                Transform organizerTransform = new GameObject(gameObjectPoolSettingsItem.name).transform;
                organizerTransform.SetParent(transform);

                gameObjectPools[gameObjectPoolSettingsItem.name.RemoveSpaces()] =
                    new GameObjectPool
                    (
                        gameObjectPoolSettingsItem.GameObject,
                        gameObjectPoolSettingsItem.Amount,
                        organizerTransform
                    );
            }
        }

        public PooledGameObject GetGameObjectFromPool(string poolName)
        {
            if (gameObjectPools.ContainsKey(poolName))
                return gameObjectPools[poolName].Release();

            return null;
        }
    }
}