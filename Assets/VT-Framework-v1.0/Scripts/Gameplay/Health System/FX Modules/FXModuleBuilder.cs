using UnityEngine;

namespace VT.Gameplay.HealthSystem.FXModules
{
    public static class FXModuleBuilder
    {
        public static WorldFXModule GetFXModule(Enums.EWorldFXModule worldFXModule, Transform fxBarTransform, Health healthSystem)
        {
            switch (worldFXModule)
            {
                case Enums.EWorldFXModule.Fade:
                    return new FadeWorldFXModule(fxBarTransform, healthSystem);
                case Enums.EWorldFXModule.Drop:
                    return new DropWorldFXModule(fxBarTransform, healthSystem);
                case Enums.EWorldFXModule.Shrink:
                    return new ShrinkWorldFXModule(fxBarTransform, healthSystem);
                default:
                case Enums.EWorldFXModule.None:
                    return null;
            }
        }

        public static UIFXModule GetFXModule(Enums.EUIFXModule worldFXModule, Transform fxBarTransform, Health healthSystem)
        {
            switch (worldFXModule)
            {
                case Enums.EUIFXModule.Fade:
                    return new FadeUIFXModule(fxBarTransform, healthSystem);
                case Enums.EUIFXModule.Drop:
                    return new DropUIFXModule(fxBarTransform, healthSystem);
                case Enums.EUIFXModule.Shrink:
                    return new ShrinkUIFXModule(fxBarTransform, healthSystem);
                default:
                case Enums.EUIFXModule.None:
                    return null;
            }
        }
    }
}