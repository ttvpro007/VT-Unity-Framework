using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VT.Utilities.GameObjectPooling;
using VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem;

public class EGameObject : MonoBehaviour
{
    public float moveSpeed;

    public Transform targetTransform;

    private void Start()
    {
        targetTransform = GameObject.Find("Target").transform;
    }

    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }
    
    void Update()
    {
        if (targetTransform && Vector3.Distance(targetTransform.position, transform.position) > 1)
        {
            transform.position += (targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Target")
        {
            PooledGameObject pooledGameObject = GetComponent<PooledGameObject>();
            pooledGameObject.SetInactive();
            PooledGameObjectIntervalSpawnerManager.Instance?.Remove(pooledGameObject);
        }
    }
}
