using System;
using System.Collections.Generic;
using UnityEngine;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    public class PooledGameObjectIntervalSpawner : MonoBehaviour
    {
        #region PUBLIC
        public event Action OnStartedSpawning;
        public event Action OnFinishedSpawning;
        public int WaveCount => pooledWaveDataSOList.Count;
        public float SpawnRadius => spawnRadius;

        public bool HasSpawnedAllInWave()
        {
            bool hasSpawnAllInWave = true;

            if (!currentWaveData)
            {
                hasSpawnAllInWave = true;
            }
            else if (currentWaveData.SpawnLimit == 0)
            {
                hasSpawnAllInWave = false;
            }
            else if (currentWaveIndex < pooledWaveDataSOList.Count)
            {
                hasSpawnAllInWave = spawnCount >= currentWaveData.SpawnLimit;
            }

            return hasSpawnAllInWave;
        }

        public void SetCanSpawn(bool value)
        {
            canSpawn = value;
        }

        public void StartWaveModeSpawning(int waveIndex)
        {
            currentWaveIndex = waveIndex;
            currentWaveData = pooledWaveDataSOList[currentWaveIndex];
            nextSpawnTime = Time.time;
            SetCanSpawn(true);
        }

        public void StartSurvivalModeSpawning(PooledSurvivalWaveDataSO pooledSurvivalWaveDataSO)
        {
            this.pooledSurvivalWaveDataSO = pooledSurvivalWaveDataSO;
            nextSpawnTime = Time.time;
            SetCanSpawn(true);
        }

        public PooledGameObject SpawnPooledGameObject(PoolType poolType, Vector3 spawnPostion)
        {
            PooledGameObject pooledGameObject = GameObjectPoolManager.Instance.GetGameObjectFromPool(poolType.ToString());

            if (!pooledGameObject) return null;

            pooledGameObject.transform.position = spawnPostion;
            pooledGameObject.SetActive();
            return pooledGameObject;
        }

        public void Reset()
        {
            spawnCount = 0;
            SetCanSpawn(false);
            doOnce = false;
        }
        #endregion

        #region PRIVATE
        [SerializeField] private float spawnRadius;
        [SerializeField] private List<PooledWaveDataSO> pooledWaveDataSOList;

        private bool canSpawn;
        private int spawnCount = 0;
        private int currentWaveIndex;
        private float nextSpawnTime;
        private PooledWaveDataSO currentWaveData;
        private PooledSurvivalWaveDataSO pooledSurvivalWaveDataSO;

        private void Update()
        {
            if (!canSpawn) return;

            if (PooledGameObjectIntervalSpawnerManager.Instance.SpawnMode == SpawnMode.Wave)
            {
                SpawnWaveMode();
            }
            else if (PooledGameObjectIntervalSpawnerManager.Instance.SpawnMode == SpawnMode.Survival)
            {
                SpawnSurvivalMode();
            }
        }

        private void SpawnWaveMode()
        {
            if (pooledWaveDataSOList == null || currentWaveData.GameObjectPool == PoolType.None) return;

            if (currentWaveIndex < pooledWaveDataSOList.Count
             && Time.time >= nextSpawnTime
             && !HasSpawnedAllInWave())
            {
                for (int i = 0; i < currentWaveData.SpawnAmount; i++)
                {
                    SpawnPooledGameObject(currentWaveData.GameObjectPool, transform.position + (Utils.GetXYRandomUnitPosition() * spawnRadius));
                    spawnCount++;

                    if (currentWaveData.SpawnLimit > 0 && HasSpawnedAllInWave())
                    {
                        OnFinishedSpawning?.Invoke();
                        break;
                    }
                }

                nextSpawnTime = Time.time + currentWaveData.SpawnInterval;
            }
        }

        private bool doOnce;
        private void SpawnSurvivalMode()
        {
            if (!pooledSurvivalWaveDataSO || pooledSurvivalWaveDataSO.GameObjectPool == PoolType.None) return;

            if (!doOnce)
            {
                OnStartedSpawning?.Invoke();
                //Audio.AudioManager.Instance.SFXPlayer.Play(SFXTrack.EnemyWaveStarting);
                doOnce = true;
            }

            if (Time.time >= nextSpawnTime && spawnCount < pooledSurvivalWaveDataSO.ModifiedSpawnAmount)
            {
                SpawnPooledGameObject(pooledSurvivalWaveDataSO.GameObjectPool, transform.position + (Utils.GetXYRandomUnitPosition() * spawnRadius));
                spawnCount++;

                if (spawnCount >= pooledSurvivalWaveDataSO.ModifiedSpawnAmount)
                {
                    Reset();
                }
                else
                {
                    nextSpawnTime = Time.time + pooledSurvivalWaveDataSO.SpawnDelay;
                }
            }
        }
        #endregion
    }
}