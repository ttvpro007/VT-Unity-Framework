using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VT.Extensions;
using VT.Utilities.Singleton;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    public class PooledGameObjectIntervalSpawnerManager : Singleton<PooledGameObjectIntervalSpawnerManager>
    {
        #region PUBLIC
        public event Action OnWaveEnded;
        public int WaveCount => waveIndex + 1;
        public float WaveCountdown => nextSpawnTime - Time.time;
        public SpawnMode SpawnMode => spawnMode;
        public PooledGameObjectIntervalSpawner NextRandomSpawner => nextRandomSpawner;
        public List<PooledGameObject> ActiveGameObjectList => activeGameObjectList;
        public int CurrentWaveSpawnAmount => pooledSurvivalWaveDataSO.ModifiedSpawnAmount;

        public void Add(PooledGameObject eGameObject)
        {
            if (activeGameObjectList == null)
            {
                activeGameObjectList = new List<PooledGameObject>();
            }

            activeGameObjectList.Add(eGameObject);
        }

        public void Remove(PooledGameObject eGameObject)
        {
            if (activeGameObjectList == null) return;

            if (activeGameObjectList.Contains(eGameObject))
            {
                activeGameObjectList.Remove(eGameObject);
            }

            if (activeGameObjectList.Count <= 0)
            {
                AllowSpawnNextWave();
            }
        }
        #endregion

        #region PROTECTED
        protected override void Awake()
        {
            base.Awake();

            pooledGameObjectIntervalSpawners = transform.GetComponentsOfTypeInHierachy<PooledGameObjectIntervalSpawner>();
        }
        #endregion

        #region PRIVATE
        [SerializeField, MinValue(0)] private float spawnStartDelay;
        [SerializeField, EnumToggleButtons, HideLabel] private SpawnMode spawnMode;
        [SerializeField, ShowIf("spawnMode", SpawnMode.Wave), MinValue(0)] private float waveInterval;
        [SerializeField, ShowIf("spawnMode", SpawnMode.Survival), InlineEditor] private PooledSurvivalWaveDataSO pooledSurvivalWaveDataSO;

        private List<PooledGameObjectIntervalSpawner> pooledGameObjectIntervalSpawners;
        private List<PooledGameObject> activeGameObjectList;

        private bool duringWave;
        private int waveIndex = 0;
        private float nextSpawnTime;
        private int maxWave;
        private PooledGameObjectIntervalSpawner nextRandomSpawner;
        private bool canSpawn;

        //private void Start()
        //{
        //    BuilderDefender.Gameplay.GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
        //    canSpawn = true;
        //}

        //private void GameManager_OnGameEnded(BuilderDefender.Gameplay.GameManager.GameState obj)
        //{
        //    canSpawn = false;
        //}

        private void OnEnable()
        {
            if (spawnMode == SpawnMode.Wave)
            {
                foreach (var pooledGameObjectIntervalSpawner in pooledGameObjectIntervalSpawners)
                {
                    pooledGameObjectIntervalSpawner.OnFinishedSpawning += PooledGameObjectIntervalSpawner_OnFinishedSpawning;
                }

                maxWave = pooledGameObjectIntervalSpawners.Where(x => x.gameObject.activeSelf).Max(y => y.WaveCount);
            }
            else if (spawnMode == SpawnMode.Survival)
            {
                if (pooledSurvivalWaveDataSO)
                    pooledSurvivalWaveDataSO.Setup();

                UpdateNextSurvivalWaveSettings();
            }

            nextSpawnTime = Time.time + spawnStartDelay;
        }

        private void Update()
        {
            if (!canSpawn) return;

            if (spawnMode == SpawnMode.Wave)
            {
                SpawnWaveMode();
            }
            else if (spawnMode == SpawnMode.Survival)
            {
                SpawnSurvivalMode();
            }
        }

        private void SpawnWaveMode()
        {
            if (!duringWave
             && waveIndex < maxWave
             && Time.time >= nextSpawnTime)
            {
                duringWave = true;

                foreach (var pooledGameObjectIntervalSpawner in pooledGameObjectIntervalSpawners)
                {
                    pooledGameObjectIntervalSpawner.StartWaveModeSpawning(waveIndex);
                }

                EndWave();
            }
        }

        private void EndWave()
        {
            if (spawnMode == SpawnMode.Survival)
            {
                UpdateNextSurvivalWaveSettings();
            }
            OnWaveEnded?.Invoke();
            waveIndex++; 
        }

        private void SpawnSurvivalMode()
        {
            if (pooledGameObjectIntervalSpawners == null) return;

            if (Time.time >= nextSpawnTime)
            {
                nextRandomSpawner.StartSurvivalModeSpawning(pooledSurvivalWaveDataSO);
                nextSpawnTime = Time.time + pooledSurvivalWaveDataSO.ModifiedSpawnInterval;
                EndWave();
            }
        }

        private void UpdateNextSurvivalWaveSettings()
        {
            nextRandomSpawner = pooledGameObjectIntervalSpawners[UnityEngine.Random.Range(0, pooledGameObjectIntervalSpawners.Count)];

            if (waveIndex > 0)
                pooledSurvivalWaveDataSO.UpdateNextWaveSettings(waveIndex);
        }

        private void AllowSpawnNextWave()
        {
            if (waveIndex < maxWave)
            {
                nextSpawnTime = Time.time + waveInterval;
            }

            duringWave = false;
        }

        private void PooledGameObjectIntervalSpawner_OnFinishedSpawning()
        {
            bool hasSpawnedAllWaves = pooledGameObjectIntervalSpawners.Where(x => x.gameObject.activeSelf).All(y => y.HasSpawnedAllInWave());

            if (hasSpawnedAllWaves)
            {
                foreach (var pooledGameObjectIntervalSpawner in pooledGameObjectIntervalSpawners)
                {
                    pooledGameObjectIntervalSpawner.Reset();
                }
            }
        }
        #endregion
    }
}