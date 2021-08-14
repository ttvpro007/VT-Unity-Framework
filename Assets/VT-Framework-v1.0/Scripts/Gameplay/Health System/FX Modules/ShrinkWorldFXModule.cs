using DG.Tweening;
using UnityEngine;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class ShrinkWorldFXModule : WorldFXModule
    {
        public ShrinkWorldFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
        }

        protected override void PlayRoutine()
        {
            if (fxBarTransform)
            {
                if (fxBarTween != null)
                    fxBarTween.Kill();

                fxBarTween = fxBarTransform.DOScaleX(healthSystem.HealthPercentageNormalized, 0.1f)
                                           .SetDelay(0.2f);
            }
        }
    }
}