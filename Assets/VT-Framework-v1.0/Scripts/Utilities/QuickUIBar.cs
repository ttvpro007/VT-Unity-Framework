using UnityEngine;

namespace VT.Utilities
{
    public class QuickUIBar : QuickBar
    {
        public QuickUIBar(Colors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(null, colors, sizeDelta, outlineThickness, parentRectTransform);
        }

        public QuickUIBar(Sprite sprite, Colors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(sprite, colors, sizeDelta, outlineThickness, parentRectTransform);
        }

        public QuickUIBar(Sprites sprites, Colors colors, Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, float outlineThickness = 0.1f, string name = "UI Bar")
        {
            sizeDelta.y = Utils.ReplaceIfZero(sizeDelta.y, 1f);

            RectTransform parentRectTransform = SetupParentRectTransform(sizeDelta, positionOffset, parent, name);
            DrawBars(sprites, colors, sizeDelta, outlineThickness, parentRectTransform);
        }

        public override void SetSize(float ratio)
        {
            foregroundTransform.localScale = new Vector3(ratio, foregroundTransform.localScale.y, foregroundTransform.localScale.z);
        }

        private RectTransform foregroundTransform = null;

        private RectTransform SetupParentRectTransform(Vector3 sizeDelta, Vector3 positionOffset, RectTransform parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.transform.localScale = Vector3.one;

            RectTransform goRectTransform = go.GetComponent<RectTransform>();
            goRectTransform.anchoredPosition = positionOffset;
            goRectTransform.sizeDelta = sizeDelta;

            return goRectTransform;
        }

        private void DrawBars(Sprite sprite, Colors colors, Vector3 sizeDelta, float outlineThickness, RectTransform parentRectTransform)
        {
            DrawOutline(sprite, colors.OutlineColor, sizeDelta, outlineThickness, parentRectTransform);
            DrawBackground(sprite, colors.BackgroundColor, sizeDelta, parentRectTransform);
            DrawForeground(sprite, colors.ForegroundColor, sizeDelta, parentRectTransform);
        }

        private void DrawBars(Sprites sprites, Colors colors, Vector3 sizeDelta, float outlineThickness, RectTransform parentRectTransform)
        {
            DrawOutline(sprites.BackgroundSprite, colors.OutlineColor, sizeDelta, outlineThickness, parentRectTransform);
            DrawBackground(sprites.BackgroundSprite, colors.BackgroundColor, sizeDelta, parentRectTransform);
            DrawForeground(sprites.ForegroundSprite, colors.ForegroundColor, sizeDelta, parentRectTransform);
        }

        private void DrawOutline(Sprite sprite, Color color, Vector3 sizeDelta, float outlineThickness, RectTransform parent, string name = "Outline")
        {
            float xyRatio = sizeDelta.x / sizeDelta.y;
            Vector3 outlineSize = new Vector3(sizeDelta.x * (1f + outlineThickness), sizeDelta.y * (1f + outlineThickness * xyRatio), sizeDelta.z);
            Utils.DrawSpriteUI(sprite, color, outlineSize, Vector3.zero, parent, name);
        }

        private void DrawBackground(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent, string name = "Background")
        {
            Utils.DrawSpriteUI(sprite, color, sizeDelta, Vector3.zero, parent, name);
        }

        private void DrawForeground(Sprite sprite, Color color, Vector3 sizeDelta, RectTransform parent, string name = "Foreground")
        {
            GameObject go = new GameObject(name, typeof(RectTransform));

            foregroundTransform = go.GetComponent<RectTransform>();
            foregroundTransform.SetParent(parent, false);
            foregroundTransform.localScale = Vector3.one;
            foregroundTransform.sizeDelta = sizeDelta;
            foregroundTransform.anchoredPosition = Vector3.zero;
            foregroundTransform.anchorMin = new Vector2(0f, 0.5f);
            foregroundTransform.anchorMax = new Vector2(0f, 0.5f);
            foregroundTransform.pivot = new Vector2(0f, 0.5f);
            
            Utils.DrawSpriteUI(sprite, color, sizeDelta, Vector3.zero, foregroundTransform, "Bar");
        }
    }
}