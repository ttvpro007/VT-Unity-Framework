using UnityEngine;

namespace VT.QuickBar
{
    [System.Serializable]
    public struct QuickBarSprites
    {
        public QuickBarSprites(Sprite sprite)
        {
            OutlineSprite = sprite;
            BackgroundSprite = sprite;
            ShrinkBarSprite = sprite;
            HealthBarSprite = sprite;
        }

        public QuickBarSprites(Sprite outlineSprite, Sprite backgroundSprite, Sprite shrinkBarSprite, Sprite healthBarSprite)
        {
            OutlineSprite = outlineSprite;
            BackgroundSprite = backgroundSprite;
            ShrinkBarSprite = shrinkBarSprite;
            HealthBarSprite = healthBarSprite;
        }

        public Sprite OutlineSprite;
        public Sprite BackgroundSprite;
        public Sprite ShrinkBarSprite;
        public Sprite HealthBarSprite;
    }
}