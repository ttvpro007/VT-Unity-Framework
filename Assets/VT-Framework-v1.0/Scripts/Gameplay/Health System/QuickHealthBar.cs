using TMPro;
using UnityEngine;
using VT.Extensions;

namespace VT.Gameplay.HealthSystem
{
    public abstract class QuickHealthBar : MonoBehaviour
    {
        public Health HealthSystem => healthSystem;
        protected Health healthSystem;
        protected TMP_Text healthText;
        protected FXModule fxModule;
        protected Material healthBarMaterial;
        [SerializeField, HideInInspector] protected QuickHealthBarPresets quickHealthBarPresets;

        protected virtual Transform healthBarTransform { get; set; }
        protected virtual Transform fxBarTransform { get; set; }

        protected abstract void SetupExtraDependencies();

        public virtual QuickHealthBar Setup(Health healthSystem)
        {
            this.healthSystem = healthSystem;
            SetupCoreDependencies();
            SetupExtraDependencies();
            UpdateHealthBarVisuals();
            return this;
        }

        private void SetupCoreDependencies()
        {
            healthSystem.OnHealthAdded += HealthSystem_OnHealthAdded;
            healthSystem.OnHealthSubtracted += HealthSystem_OnHealthSubtracted;
            healthText = GetComponentInChildren<TMP_Text>();
            healthBarTransform = transform.GetFirstComponentOfTypeInHierachy<Transform>(t => t.name == "Health Bar");
            fxBarTransform = transform.GetFirstComponentOfTypeInHierachy<Transform>(t => t.name == "FX Bar");
        }

        public void SetPresets(QuickHealthBarPresets quickHealthBarPresets)
        {
            this.quickHealthBarPresets = quickHealthBarPresets;
        }

        protected virtual void HealthSystem_OnHealthAdded()
        {
            UpdateHealthBarVisuals();
        }

        protected virtual void HealthSystem_OnHealthSubtracted()
        {
            UpdateHealthBarVisuals();
        }

        protected virtual void UpdateHealthBarVisuals()
        {
            if (healthText)
                healthText.text = $"{(int)healthSystem.CurrentHealth}/{(int)healthSystem.MaxHealth}";

            if (healthBarMaterial)
                healthBarMaterial.SetFloat("_Progress", healthSystem.HealthPercentageNormalized);
        }

        protected virtual void OnDestroy()
        {
            if (healthSystem != null)
            {
                healthSystem.OnHealthAdded -= HealthSystem_OnHealthAdded;
                healthSystem.OnHealthSubtracted -= HealthSystem_OnHealthSubtracted;
            }
        }
    }
}