using UnityEditor;
using UnityEngine;
using VT.Extensions;
using VT.Utilities.Factory;

namespace VT.Utilities.GameObjectPooling
{
    public static class GameObjectPoolUtilities
    {
        [MenuItem("GameObject/VT/GameObject Pooling/GameObject Pool Manager", false, 10)]
        private static void CreateGameObjectPoolManager(MenuCommand menuCommand)
        {
            GameObject go = VTObjectFactory.CreateEditorGameObject("GameObject Pool Manager").SetParentAndAlign((GameObject)menuCommand.context);
            go.AddComponent<GameObjectPoolManager>();
        }
    }
}