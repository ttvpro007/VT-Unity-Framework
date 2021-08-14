using System;
using UnityEngine;

namespace VT.Gameplay.HealthSystem
{
    [Serializable]
    public class Health
    {
        #region PUBLIC
        public bool IsAlive => currentHealth > 0;
        public bool IsFullHealth => currentHealth == maxHealth;
        public float HealthPercentageNormalized => currentHealth / maxHealth;
        public float LastHealthModifier { get; private set; }
        public float LastHealthModifierPercentageNormalized => LastHealthModifier / MaxHealth;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public float MissingHealth => maxHealth - currentHealth;

        public event Action OnHealthAdded;
        public event Action OnHealthSubtracted;
        public event Action OnDead;

        public static Health Create(int startHealth, int maxHealth)
        {
            return new Health(startHealth, maxHealth);
        }

        public Health()
        {
            startHealth = defaultHealth;
            maxHealth = startHealth;
            ResetHealth();
        }

        public Health(int startHealth, int maxHealth)
        {
            this.startHealth = Mathf.Max(1, startHealth);
            this.maxHealth = Mathf.Max(startHealth, maxHealth);
            ResetHealth();
        }

        public void SetHealth(float amount)
        {
            currentHealth = Mathf.Clamp(amount, 0, maxHealth);
        }

        public void AddHealth(float amount)
        {
            ModifyHealth(amount);
            OnHealthAdded?.Invoke();
        }

        public void SubtractHealth(float amount)
        {
            ModifyHealth(-amount);
            OnHealthSubtracted?.Invoke();
        }

        public void ResetHealth()
        {
            currentHealth = startHealth;
        }
        #endregion

        #region PRIVATE
        private void ModifyHealth(float amount)
        {
            LastHealthModifier = amount;
            SetHealth(currentHealth + amount);
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private static readonly int defaultHealth = 100;

        [SerializeField]
        private int startHealth;
        [SerializeField]
        private int maxHealth;
        [SerializeField]
        private float currentHealth;

        private void Die()
        {
            currentHealth = 0;
            OnDead?.Invoke();
        }
        #endregion
    }
}