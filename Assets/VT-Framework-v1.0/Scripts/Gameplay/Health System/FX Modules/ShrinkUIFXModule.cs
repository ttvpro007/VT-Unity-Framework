using DG.Tweening;
using UnityEngine;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class ShrinkUIFXModule : UIFXModule
    {
        public ShrinkUIFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
        }

        protected override void PlayRoutine()
        {
            if (fxBarImage)
            {
                if (fxBarTween != null)
                    fxBarTween.Kill();

                fxBarTween = fxBarImage.DOFillAmount(healthSystem.HealthPercentageNormalized, 0.1f)
                                       .SetDelay(0.2f);
            }
        }
    }
}