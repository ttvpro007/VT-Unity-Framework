using UnityEngine;
using VT.Extensions;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public abstract class WorldFXModule : FXModule
    {
        protected SpriteRenderer fxBarSpriteRenderer;
        public WorldFXModule(Transform fxBarTransform, Health healthSystem) : base(fxBarTransform, healthSystem)
        {
            if (fxBarTransform)
            {
                fxBarSpriteRenderer = fxBarTransform.GetComponentInChildren<SpriteRenderer>();
            }
        }

        protected override void PlayRoutine()
        { 
        }

        protected override void HealthSystem_OnHealthAdded()
        {
            base.HealthSystem_OnHealthAdded();

            if (fxBarTransform)
                fxBarTransform.SetSizeX(healthSystem.HealthPercentageNormalized);
        }
    }
}