using UnityEngine;

namespace VT.QuickBar
{
    [System.Serializable]
    public struct QuickBarColors
    {
        public QuickBarColors(Color outlineColor, Color backgroundColor, Color foregroundColor)
        {
            OutlineColor = outlineColor;
            BackgroundColor = backgroundColor;
            ShrinkBarColor = Color.white;
            ShrinkBarColor.a = 0;
            HealthBarColor = foregroundColor;
        }

        public QuickBarColors(Color outlineColor, Color backgroundColor, Color followBarColor, Color foregroundColor)
        {
            OutlineColor = outlineColor;
            BackgroundColor = backgroundColor;
            ShrinkBarColor = followBarColor;
            HealthBarColor = foregroundColor;
        }

        public Color OutlineColor;
        public Color BackgroundColor;
        public Color ShrinkBarColor;
        public Color HealthBarColor;
    }
}