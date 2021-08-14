using UnityEngine;
using UnityEngine.UI;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public abstract class UIFXModule : FXModule
    {
        protected Image fxBarImage;

        protected UIFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
            if (fxBarTransform)
            {
                fxBarImage = fxBarTransform.GetComponentInChildren<Image>();
            }
        }

        protected override void PlayRoutine()
        {
        }

        protected override void HealthSystem_OnHealthAdded()
        {
            base.HealthSystem_OnHealthAdded();

            if (fxBarImage)
                fxBarImage.fillAmount = healthSystem.HealthPercentageNormalized;
        }
    }
}