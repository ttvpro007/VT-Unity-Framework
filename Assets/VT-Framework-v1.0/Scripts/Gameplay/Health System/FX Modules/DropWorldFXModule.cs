using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VT.Extensions;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class DropWorldFXModule : WorldFXModule
    {
        public DropWorldFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
            barVisualSize = fxBarTransform.GetChild(0).localScale;
        }

        protected override void PlayRoutine()
        {
            if (fxBarTransform)
            {
                float halfSizeX = barVisualSize.x / 2f;
                Transform dropBarTransform = Object.Instantiate(fxBarTransform, fxBarTransform.parent);
                dropBarTransform.localScale = new Vector3(Mathf.Abs(healthSystem.LastHealthModifierPercentageNormalized), fxBarTransform.localScale.y, fxBarTransform.localScale.z);
                dropBarTransform.localPosition = new Vector3(Mathf.Lerp(-halfSizeX, halfSizeX, healthSystem.HealthPercentageNormalized), 0f, 0f);

                Task dropTask = dropBarTransform.DOMoveY(-barVisualSize.y * 1.5f, 1f)
                                                .SetRelative()
                                                .SetDelay(0.2f).AsyncWaitForCompletion();

                SpriteRenderer sr = dropBarTransform.GetComponentInChildren<SpriteRenderer>();
                if (sr)
                {
                    sr.DOFade(0, 1f).From(1).SetDelay(0.2f).OnComplete(async () =>
                    {
                        await dropTask;
                        Object.Destroy(dropBarTransform.gameObject);
                    });
                }
            }

            if (fxBarTransform)
                fxBarTransform.SetSizeX(healthSystem.HealthPercentageNormalized);
        }

        private Vector3 barVisualSize;
    }
}