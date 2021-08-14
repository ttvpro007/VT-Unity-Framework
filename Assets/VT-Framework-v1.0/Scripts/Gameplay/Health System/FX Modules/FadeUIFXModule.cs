using DG.Tweening;
using UnityEngine;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class FadeUIFXModule : UIFXModule
    {
        public FadeUIFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
        }

        protected override void PlayRoutine()
        {
            if (fxBarTransform)
            {
                if (fxBarImage)
                {
                    if (fxBarTween != null)
                        fxBarTween.Kill();

                    fxBarTween = fxBarImage
                    .DOFade(0, 0.2f).From(1).SetDelay(0.3f)
                    .OnComplete(() => fxBarImage.DOFillAmount(healthSystem.HealthPercentageNormalized, 0f)
                                                .From(fxBarTransform.localScale.x));
                }
            }
        }
    }
}