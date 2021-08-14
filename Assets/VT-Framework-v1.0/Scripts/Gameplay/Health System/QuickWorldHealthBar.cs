using UnityEngine;
using VT.Extensions;

namespace VT.Gameplay.HealthSystem
{
    public class QuickWorldHealthBar : QuickHealthBar
    {
        #region PROTECTED
        protected FXModules.Enums.EWorldFXModule damageFXModule;

        protected override void SetupExtraDependencies()
        {
            if (healthBarTransform)
            {
                healthBarSpriteRenderer = healthBarTransform.GetComponentInChildren<SpriteRenderer>();
            }

            if (healthBarSpriteRenderer)
            {
                healthBarMaterial = healthBarSpriteRenderer.material;
            }
            
            alwaysShow = quickHealthBarPresets.AlwaysShow;

            if (fxBarTransform)
            {
                damageFXModule = quickHealthBarPresets.WorldDamageFX;
                fxModule = FXModules.FXModuleBuilder.GetFXModule(damageFXModule, fxBarTransform, healthSystem);
            }
        }

        protected override void UpdateHealthBarVisuals()
        {
            base.UpdateHealthBarVisuals();

            if (!alwaysShow)
                gameObject.SetActive(!healthSystem.IsFullHealth);

            if (healthBarTransform)
                healthBarTransform.SetSizeX(healthSystem.HealthPercentageNormalized);
        }
        #endregion

        #region PRIVATE
        private SpriteRenderer healthBarSpriteRenderer;
        private bool alwaysShow;
        #endregion
    }
}