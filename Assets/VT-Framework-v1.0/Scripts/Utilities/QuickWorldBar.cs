using UnityEngine;

namespace VT.Utilities
{
    public class QuickWorldBar : QuickBar
    {
        public QuickWorldBar(Sprite sprite, Colors colors, Vector3 size, Vector3 positionOffset, Transform parent, float outlineThickness = 0.1f, string name = "World Bar")
        {
            outlineThickness = Mathf.Max(0f, outlineThickness);
            size.y = Utils.ReplaceIfZero(size.y, 1f);

            Transform parentTransform = SetupParentTransform(positionOffset, parent, name);
            DrawBars(sprite, colors, size, outlineThickness, parentTransform);
        }

        public QuickWorldBar(Sprites sprites, Colors colors, Vector3 size, Vector3 positionOffset, Transform parent, float outlineThickness = 0.1f, string name = "World Bar")
        {
            outlineThickness = Mathf.Max(0f, outlineThickness);
            size.y = Utils.ReplaceIfZero(size.y, 1f);

            Transform parentTransform = SetupParentTransform(positionOffset, parent, name);
            DrawBars(sprites, colors, size, outlineThickness, parentTransform);
        }

        public override void SetSize(float ratio)
        {
            foregroundTransform.localScale = new Vector3(ratio, foregroundTransform.localScale.y, foregroundTransform.localScale.z);
        }

        private Transform foregroundTransform = null;

        private Transform SetupParentTransform(Vector3 positionOffset, Transform parent, string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = positionOffset;
            return go.transform;
        }

        private void DrawBars(Sprite sprite, Colors colors, Vector3 size, float outlineThickness, Transform parentTransform)
        {
            DrawOutline(sprite, colors.OutlineColor, size, outlineThickness, parentTransform);
            DrawBackground(sprite, colors.BackgroundColor, size, parentTransform);
            DrawForeground(sprite, colors.ForegroundColor, size, parentTransform);
        }

        private void DrawBars(Sprites sprites, Colors colors, Vector3 size, float outlineThickness, Transform parentTransform)
        {
            DrawOutline(sprites.OutlineSprite, colors.OutlineColor, size, outlineThickness, parentTransform);
            DrawBackground(sprites.BackgroundSprite, colors.BackgroundColor, size, parentTransform);
            DrawForeground(sprites.ForegroundSprite, colors.ForegroundColor, size, parentTransform);
        }

        private void DrawOutline(Sprite sprite, Color color, Vector3 size, float outlineThickness, Transform parent, string name = "Outline")
        {
            float xyRatio = size.x / size.y;
            Vector3 outlineSize = new Vector3(size.x * (1f + outlineThickness), size.y * (1f + outlineThickness * xyRatio), size.z);
            Utils.DrawSprite(sprite, color, outlineSize, Vector3.zero, parent, name);
        }

        private void DrawBackground(Sprite sprite, Color color, Vector3 size, Transform parent, string name = "Background")
        {
            Utils.DrawSprite(sprite, color, size, Vector3.forward * zOffset, parent, name);
        }

        private void DrawForeground(Sprite sprite, Color color, Vector3 size, Transform parent, string name = "Foreground")
        {
            GameObject go = new GameObject(name);
            foregroundTransform = go.transform;
            foregroundTransform.SetParent(parent, false);
            foregroundTransform.localPosition = new Vector3(-size.x / (2f * sprite.pixelsPerUnit), 0f, zOffset * 2);
            Utils.DrawSprite(sprite, color, size, -foregroundTransform.localPosition + Vector3.forward * foregroundTransform.localPosition.z, foregroundTransform, "Bar");
        }
    }
}