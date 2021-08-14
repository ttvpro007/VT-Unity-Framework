using Sirenix.OdinInspector;
using UnityEngine;

namespace VT.Utilities.GameObjectPooling.PooledGameObjectSpawnSystem
{
    [CreateAssetMenu(fileName = "New Pooled Survival Wave Data SO", menuName = "VT/Spawn System/Create Pooled Survival Wave Data SO")]
    public class PooledSurvivalWaveDataSO : PooledWaveDataSO
    {
        #region PUBLIC
        public float SpawnDelay => spawnDelay;
        public int ModifiedSpawnAmount => Mathf.RoundToInt(modifiedSpawnAmount);
        public float ModifiedSpawnInterval => modifiedSpawnInterval;

        public void Setup()
        {
            modifiedSpawnAmount = spawnAmount;
            modifiedSpawnInterval = spawnInterval;
        }

        public void UpdateNextWaveSettings(int waveIndex)
        {
            modifiedSpawnAmount += modifiedSpawnAmount * spawnAmountIncrementPercentage;
            modifiedSpawnInterval += waveIndex * spawnIntervalIncrementPercentage;
        }
        #endregion

        #region PRIVATE
        [SerializeField, MinValue(0)] private float spawnAmountIncrementPercentage;
        [SerializeField, MinValue(0)] private float spawnIntervalIncrementPercentage;
        [SerializeField, MinValue(0)] private float spawnDelay;

        private float modifiedSpawnAmount;
        private float modifiedSpawnInterval;
        #endregion
    }
}