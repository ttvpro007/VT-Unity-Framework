//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EGameObjectManager : Singleton<EGameObjectManager>
//{
//    public int ActiveGameObjectCount => activeGameObjectList != null ? activeGameObjectList.Count : 0;
//    public event Action OnActiveGameObjectCountReachZero;

//    private List<EGameObject> activeGameObjectList;

//    public void Add(EGameObject eGameObject)
//    {
//        if (activeGameObjectList == null)
//        {
//            activeGameObjectList = new List<EGameObject>();
//        }

//        activeGameObjectList.Add(eGameObject);
//    }

//    public void Remove(EGameObject eGameObject)
//    {
//        if (activeGameObjectList.Contains(eGameObject))
//        {
//            activeGameObjectList.Remove(eGameObject);
//        }

//        if (ActiveGameObjectCount <= 0)
//        {
//            OnActiveGameObjectCountReachZero?.Invoke();
//        }
//    }
//}
