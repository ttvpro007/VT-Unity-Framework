using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VT.Utilities.Singleton;

public class ESpawnerManager : Singleton<ESpawnerManager>
{
    private List<EGameObject> activeGameObjectList;

    public void Add(EGameObject eGameObject)
    {
        if (activeGameObjectList == null)
        {
            activeGameObjectList = new List<EGameObject>();
        }

        activeGameObjectList.Add(eGameObject);
    }

    public void Remove(EGameObject eGameObject)
    {
        if (activeGameObjectList.Contains(eGameObject))
        {
            activeGameObjectList.Remove(eGameObject);
        }

        if (activeGameObjectList.Count <= 0)
        {
            AllowSpawnNextWave();
        }
    }

    public List<ESpawner> eSpawners;
    public float waveInterval;

    private bool duringWave;
    private int waveIndex = 0;
    private float nextSpawnTime;
    private int maxWave;

    private void OnEnable()
    {
        foreach (var eSpawner in eSpawners)
        {
            eSpawner.OnFinishedSpawning += ESpawner_OnFinishedSpawning;
        }

        maxWave = eSpawners.Where(x => x.gameObject.activeSelf).Max(y => y.eSpawnSettings.Count);
    }

    private void AllowSpawnNextWave()
    {
        if (waveIndex < maxWave)
        {
            nextSpawnTime = Time.time + waveInterval;
        }
        
        duringWave = false;
    }

    private void ESpawner_OnFinishedSpawning()
    {
        bool hasSpawnedAllWaves = eSpawners.Where(x => x.gameObject.activeSelf).All(y => y.HasSpawnedAllInWave());

        if (hasSpawnedAllWaves)
        {
            foreach (var eSpawner in eSpawners)
            {
                eSpawner.Reset();
            }
        }
    }

    void Update()
    {
        if (duringWave) return;
        
        if (Time.time >= nextSpawnTime && waveIndex < maxWave)
        {
            duringWave = true;
            
            foreach (var eSpawner in eSpawners)
            {
                eSpawner.SetWaveSettings(waveIndex);
            }

            waveIndex++;
        }
    }
}
