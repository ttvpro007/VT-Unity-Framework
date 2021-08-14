using System;
using System.Collections.Generic;
using UnityEngine;
using VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem;

public class ESpawner : MonoBehaviour
{
    public float spawnRadius;
    public List<PooledWaveDataSO> eSpawnSettings;

    private bool canSpawn;
    private int spawnCount = 0;
    private int currentWaveIndex;

    public event Action OnFinishedSpawning;

    public bool HasSpawnedAllInWave()
    {
        bool hasSpawnAllInWave = true;

        if (currentWaveIndex < eSpawnSettings.Count)
        {
            hasSpawnAllInWave = spawnCount >= eSpawnSettings[currentWaveIndex].SpawnLimit;
        }

        return hasSpawnAllInWave;
    }

    public void SetCanSpawn(bool value)
    {
        canSpawn = value;
    }

    public void SetWaveSettings(int waveIndex)
    {
        currentWaveIndex = waveIndex;
        nextSpawnTime = Time.time;
        SetCanSpawn(true);
    }

    public void UpdateNextSpawnTime()
    {
        nextSpawnTime = Time.time + eSpawnSettings[currentWaveIndex].SpawnInterval;
    }

    private float nextSpawnTime;

    private void Update()
    {
        if (!canSpawn) return;

        if (currentWaveIndex < eSpawnSettings.Count)
        {
            if (Time.time >= nextSpawnTime && !HasSpawnedAllInWave())
            {
                for (int i = 0; i < eSpawnSettings[currentWaveIndex].SpawnAmount; i++)
                {
                    //EGameObject eGameObject = Instantiate(eSpawnSettings[currentWaveIndex].PoolType, transform.position + (GetRandomPosition() * spawnRadius), Quaternion.identity).GetComponent<EGameObject>();
                    spawnCount++;
                    //ESpawnerManager.Instance.Add(eGameObject);

                    if (eSpawnSettings[currentWaveIndex].SpawnLimit > 0 && HasSpawnedAllInWave())
                    {
                        OnFinishedSpawning?.Invoke();
                        break;
                    }
                }

                UpdateNextSpawnTime();
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = UnityEngine.Random.insideUnitCircle;
        randomPosition.z = randomPosition.y;
        randomPosition.y = 0;
        return randomPosition;
    }

    public void Reset()
    {
        spawnCount = 0;
        SetCanSpawn(false);
    }
}