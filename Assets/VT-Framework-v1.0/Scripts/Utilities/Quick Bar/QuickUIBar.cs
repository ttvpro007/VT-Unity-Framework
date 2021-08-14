using UnityEngine;
using UnityEngine.UI;
using VT.Utilities;

namespace VT.QuickBar
{
#if UNITY_EDITOR
    public static class QuickUIBar
    {
        #region PUBLIC
        public static GameObject Create(QuickBarColors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, Material material, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(null, colors, sizeDelta, outlineThickness, parentRectTransform, material);

            return parentRectTransform.gameObject;
        }

        public static GameObject Create(Sprite sprite, QuickBarColors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, Material material, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(sprite, colors, sizeDelta, outlineThickness, parentRectTransform, material);

            return parentRectTransform.gameObject;
        }

        public static GameObject Create(QuickBarSprites sprites, QuickBarColors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, Material material, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(sprites, colors, sizeDelta, outlineThickness, parentRectTransform, material);

            return parentRectTransform.gameObject;
        }
        #endregion

        #region PRIVATE
        private static RectTransform SetupParentRectTransform(Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.transform.localScale = Vector3.one;
            // Set to UI(5) layer
            go.layer = 5;

            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.anchoredPosition = positionOffset;
            goRectTransform.sizeDelta = sizeDelta;

            return goRectTransform;
        }

        private static void DrawBars(Sprite sprite, QuickBarColors colors, Vector3 sizeDelta, float outlineThickness, RectTransform parentRectTransform, Material healthBarMaterial)
        {
            DrawOutline(sprite, colors.OutlineColor, sizeDelta, outlineThickness, parentRectTransform);
            DrawBackground(sprite, colors.BackgroundColor, sizeDelta, parentRectTransform);
            DrawShrinkBar(sprite, colors.ShrinkBarColor, sizeDelta, parentRectTransform);
            DrawHealthBar(sprite, colors.HealthBarColor, sizeDelta, parentRectTransform, healthBarMaterial);
        }

        private static void DrawBars(QuickBarSprites sprites, QuickBarColors colors, Vector3 sizeDelta, float outlineThickness, RectTransform parentRectTransform, Material healthBarMaterial)
        {
            DrawOutline(sprites.BackgroundSprite, colors.OutlineColor, sizeDelta, outlineThickness, parentRectTransform);
            DrawBackground(sprites.BackgroundSprite, colors.BackgroundColor, sizeDelta, parentRectTransform);
            DrawShrinkBar(sprites.ShrinkBarSprite, colors.ShrinkBarColor, sizeDelta, parentRectTransform);
            DrawHealthBar(sprites.HealthBarSprite, colors.HealthBarColor, sizeDelta, parentRectTransform, healthBarMaterial);
        }

        private static void DrawOutline(Sprite sprite, Color color, Vector3 sizeDelta, float outlineThickness, RectTransform parent)
        {
            // use x:y ratio to make outline uniform
            float xyRatio = sizeDelta.x / sizeDelta.y;
            Vector3 outlineSize = new Vector3(sizeDelta.x * (1f + outlineThickness), sizeDelta.y * (1f + outlineThickness * xyRatio), sizeDelta.z);
            Utils.DrawImage(sprite, color, outlineSize, Vector3.zero, parent, null, "Outline");
        }

        private static void DrawBackground(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent)
        {
            Utils.DrawImage(sprite, color, sizeDelta, Vector3.zero, parent, null, "Background");
        }

        private static void DrawShrinkBar(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent)
        {
            DrawAnchoredLeftBar(sprite, color, sizeDelta, parent, null, "FX Bar");
        }

        private static void DrawHealthBar(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent, Material material)
        {
            DrawAnchoredLeftBar(sprite, color, sizeDelta, parent, material, "Health Bar");
        }

        private static void DrawAnchoredLeftBar(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent, Material material, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            // Set to UI(5) layer
            go.layer = 5;

            RectTransform foregroundTransform = go.GetComponent<RectTransform>();
            foregroundTransform.SetParent(parent, false);
            foregroundTransform.localScale = Vector3.one;
            foregroundTransform.sizeDelta = sizeDelta;
            foregroundTransform.anchoredPosition = Vector3.zero;
            foregroundTransform.anchorMin = new Vector2(0f, 0.5f);
            foregroundTransform.anchorMax = new Vector2(0f, 0.5f);
            foregroundTransform.pivot = new Vector2(0f, 0.5f);

            Image i = Utils.DrawImage(sprite, color, sizeDelta, Vector3.zero, foregroundTransform, material, "Bar Visual");
            i.type = Image.Type.Filled;
            i.fillMethod = Image.FillMethod.Horizontal;
        }
        #endregion
    }
#endif
}