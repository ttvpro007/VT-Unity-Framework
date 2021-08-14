using UnityEditor;
using UnityEngine;
using VT.Extensions;
using VT.Utilities.Factory;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    public static class PooledSpawnSystemUtilities
    {
        [MenuItem("GameObject/VT/GameObject Pooling/Pooled Spawn System/Pooled Interval Spawner", false, 10)]
        private static void CreatePooledSpawner(MenuCommand menuCommand)
        {
            GameObject go = VTObjectFactory.CreateEditorGameObject("Pooled Interval Spawner").SetParentAndAlign((GameObject)menuCommand.context);
            go.AddComponent<PooledGameObjectIntervalSpawner>();
        }

        [MenuItem("GameObject/VT/GameObject Pooling/Pooled Spawn System/Pooled Interval Spawner Manager", false, 10)]
        private static void CreatePooledSpawnerManager(MenuCommand menuCommand)
        {
            GameObject go = VTObjectFactory.CreateEditorGameObject("Pooled Interval Spawner Manager").SetParentAndAlign((GameObject)menuCommand.context);
            go.AddComponent<PooledGameObjectIntervalSpawnerManager>();
        }
    }
}