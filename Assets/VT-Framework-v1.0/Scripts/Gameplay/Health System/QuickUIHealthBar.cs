using UnityEngine.UI;

namespace VT.Gameplay.HealthSystem
{
    public class QuickUIHealthBar : QuickHealthBar
    {
        #region PROTECTED
        protected FXModules.Enums.EUIFXModule damageFXModule;

        protected override void SetupExtraDependencies()
        {
            if (healthBarTransform)
            {
                healthBarImage = healthBarTransform.GetComponentInChildren<Image>();
            }
            
            if (healthBarImage)
            {
                healthBarMaterial = healthBarImage.material;
            }

            if (fxBarTransform)
            {
                damageFXModule = quickHealthBarPresets.UIDamageFX;
                fxModule = FXModules.FXModuleBuilder.GetFXModule(damageFXModule, fxBarTransform, healthSystem);
            }
        }

        protected override void UpdateHealthBarVisuals()
        {
            base.UpdateHealthBarVisuals();

            if (healthBarImage)
                healthBarImage.fillAmount = healthSystem.HealthPercentageNormalized;
        }
        #endregion

        #region PRIVATE
        private Image healthBarImage;
        #endregion
    }
}