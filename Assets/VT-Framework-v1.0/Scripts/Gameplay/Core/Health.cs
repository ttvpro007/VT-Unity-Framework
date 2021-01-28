using System;
using UnityEngine;

namespace VT.Gameplay.Core
{
    public class Health
    {
        public bool IsAlive => currentHealth > 0;
        public float HealthPercentage => (float) currentHealth / maxHealth;
        public float LastHealthModifierValue { get; private set; }

        public event Action OnHealthChanged;
        public event Action OnDead;

        public Health()
        {
            maxHealth = 100;
            currentHealth = maxHealth;
        }

        public Health(int startHealth, int maxHealth)
        {
            startHealth = Mathf.Max(1, startHealth);
            maxHealth = Mathf.Max(startHealth, maxHealth);
            currentHealth = startHealth;
        }

        public void ModifyHealth(int value)
        {
            LastHealthModifierValue = value;
            currentHealth += value;
            OnHealthChanged?.Invoke();

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth <= 0)
            {
                Die();
            }
        }

        private int maxHealth;
        private int currentHealth;

        private void Die()
        {
            currentHealth = 0;
            OnDead?.Invoke();
        }
    }
}