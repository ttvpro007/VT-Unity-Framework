using UnityEngine;

namespace VT.Utilities
{
    public abstract class QuickBar
    {
        public struct Sprites
        {
            public Sprites(Sprite sprite)
            {
                OutlineSprite = sprite;
                BackgroundSprite = sprite;
                ForegroundSprite = sprite;
            }

            public Sprites(Sprite outlineSprite, Sprite backgroundSprite, Sprite foregroundSprite)
            {
                OutlineSprite = outlineSprite;
                BackgroundSprite = backgroundSprite;
                ForegroundSprite = foregroundSprite;
            }

            public Sprite OutlineSprite;
            public Sprite BackgroundSprite;
            public Sprite ForegroundSprite;
        }

        public struct Colors
        {
            public Colors(Color outlineColor, Color backgroundColor, Color foregroundColor)
            {
                OutlineColor = outlineColor;
                BackgroundColor = backgroundColor;
                ForegroundColor = foregroundColor;
            }

            public Color OutlineColor;
            public Color BackgroundColor;
            public Color ForegroundColor;
        }

        public abstract void SetSize(float ratio);

        protected readonly float zOffset = -0.1f;
    }
}