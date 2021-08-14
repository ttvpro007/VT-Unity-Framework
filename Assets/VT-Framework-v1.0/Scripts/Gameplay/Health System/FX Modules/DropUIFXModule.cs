using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public class DropUIFXModule : UIFXModule
    {
        public DropUIFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
            barVisualSize = ((RectTransform)fxBarTransform.GetChild(0)).sizeDelta;
        }

        protected override void PlayRoutine()
        {
            if (fxBarTransform)
            {
                float halfSizeX = barVisualSize.x / 2f;
                Transform dropBarTransform = Object.Instantiate(fxBarTransform, fxBarTransform.parent);
                dropBarTransform.localPosition = new Vector3(Mathf.Lerp(-halfSizeX, halfSizeX, healthSystem.HealthPercentageNormalized), 0f, 0f);

                Task dropTask = dropBarTransform.DOMoveY(-barVisualSize.y * 1.5f, 1f)
                                                .SetRelative()
                                                .SetDelay(0.2f).AsyncWaitForCompletion();

                Image i = dropBarTransform.GetComponentInChildren<Image>();
                if (i)
                {
                    i.fillAmount = Mathf.Abs(healthSystem.LastHealthModifierPercentageNormalized);
                    i.DOFade(0, 1f).From(1).SetDelay(0.2f).OnComplete(async () =>
                    {
                        await dropTask;
                        Object.Destroy(dropBarTransform.gameObject);
                    });
                }
            }

            if (fxBarImage)
                fxBarImage.fillAmount = healthSystem.HealthPercentageNormalized;
        }

        private Vector2 barVisualSize;
    }
}