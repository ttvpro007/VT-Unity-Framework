using VT.Utilities.GameObjectPooling;
using UnityEngine;
using Sirenix.OdinInspector;

public class GOPTest : MonoBehaviour
{
    public GameObject poolGameObject;
    public int amount;
    private GameObjectPool gop;

    private void Start()
    {
        gop = new GameObjectPool(poolGameObject, amount, transform);
    }

    [Button]
    private void Spawn()
    {
        PooledGameObject spawnedGO = gop.Release();
        spawnedGO.SetActive();
    }
}