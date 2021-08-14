using DG.Tweening;
using UnityEngine;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class FadeWorldFXModule : WorldFXModule
    {
        public FadeWorldFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
        }

        protected override void PlayRoutine()
        {
            if (fxBarTransform)
            {
                if (fxBarSpriteRenderer)
                {
                    if (fxBarTween != null)
                        fxBarTween.Kill();

                    fxBarTween = fxBarSpriteRenderer
                    .DOFade(0, 0.2f).From(1).SetDelay(0.3f)
                    .OnComplete(() => fxBarTransform.DOScaleX(healthSystem.HealthPercentageNormalized, 0f)
                                                    .From(fxBarTransform.localScale.x));
                }
            }
        }

    }
}