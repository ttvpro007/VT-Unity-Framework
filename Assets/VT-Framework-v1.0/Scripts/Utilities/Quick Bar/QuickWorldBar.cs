using UnityEngine;
using VT.Utilities;

namespace VT.QuickBar
{
#if UNITY_EDITOR
    [System.Serializable]
    public static class QuickWorldBar
    {
        #region PUBLIC
        public static GameObject Create(Sprite sprite, QuickBarColors colors, Vector3 size, Vector3 positionOffset, Transform parent, Material foregroundMaterial = null, float outlineThickness = 0.1f, string sortingLayerName = default, int orderInLayer = default, string name = "World Bar")
        {
            outlineThickness = Mathf.Max(0f, outlineThickness);
            size.y = Utils.ReplaceIfZero(size.y, 1f);

            Transform parentTransform = SetupParentTransform(positionOffset, parent, name);
            DrawBars(sprite, colors, size, outlineThickness, parentTransform, foregroundMaterial, sortingLayerName, orderInLayer);

            return parentTransform.gameObject;
        }

        public static GameObject Create(QuickBarSprites sprites, QuickBarColors colors, Vector3 size, Vector3 positionOffset, Transform parent, Material foregroundMaterial = null, float outlineThickness = 0.1f, string sortingLayerName = default, int orderInLayer = default, string name = "World Bar")
        {
            outlineThickness = Mathf.Max(0f, outlineThickness);
            size.y = Utils.ReplaceIfZero(size.y, 1f);

            Transform parentTransform = SetupParentTransform(positionOffset, parent, name);
            DrawBars(sprites, colors, size, outlineThickness, parentTransform, foregroundMaterial, sortingLayerName, orderInLayer);
            
            return parentTransform.gameObject;
        }
        #endregion

        #region PRIVATE
        private static Transform SetupParentTransform(Vector3 positionOffset, Transform parent, string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = positionOffset;
            return go.transform;
        }

        private static void DrawBars(Sprite sprite, QuickBarColors colors, Vector3 size, float outlineThickness, Transform parentTransform, Material foregroundMaterial, string sortingLayerName, int orderInLayer)
        {
            DrawOutline(sprite, colors.OutlineColor, size, outlineThickness, parentTransform, sortingLayerName, orderInLayer);
            DrawBackground(sprite, colors.BackgroundColor, size, parentTransform, sortingLayerName, ++orderInLayer);
            DrawShrinkBar(sprite, colors.ShrinkBarColor, size, parentTransform, sortingLayerName, ++orderInLayer);
            DrawHealthBar(sprite, colors.HealthBarColor, size, parentTransform, foregroundMaterial, sortingLayerName, ++orderInLayer);
        }

        private static void DrawBars(QuickBarSprites sprites, QuickBarColors colors, Vector3 size, float outlineThickness, Transform parentTransform, Material foregroundMaterial, string sortingLayerName, int orderInLayer)
        {
            DrawOutline(sprites.OutlineSprite, colors.OutlineColor, size, outlineThickness, parentTransform, sortingLayerName, orderInLayer);
            DrawBackground(sprites.BackgroundSprite, colors.BackgroundColor, size, parentTransform, sortingLayerName, ++orderInLayer);
            DrawShrinkBar(sprites.HealthBarSprite, colors.ShrinkBarColor, size, parentTransform, sortingLayerName, ++orderInLayer);
            DrawHealthBar(sprites.HealthBarSprite, colors.HealthBarColor, size, parentTransform, foregroundMaterial, sortingLayerName, ++orderInLayer);
        }

        private static void DrawOutline(Sprite sprite, Color color, Vector3 size, float outlineThickness, Transform parent, string sortingLayerName, int orderInLayer)
        {
            // use x:y ratio to make outline uniform
            float xyRatio = size.x / size.y;
            Vector3 outlineSize = new Vector3(size.x * (1f + outlineThickness), size.y * (1f + outlineThickness * xyRatio), size.z);
            Utils.DrawSprite(sprite, color, outlineSize, Vector3.zero, parent, null, sortingLayerName, orderInLayer, "Outline");
        }

        private static void DrawBackground(Sprite sprite, Color color, Vector3 size, Transform parent, string sortingLayerName, int orderInLayer)
        {
            Utils.DrawSprite(sprite, color, size, Vector3.zero, parent, null, sortingLayerName, orderInLayer, "Background");
        }

        private static void DrawShrinkBar(Sprite sprite, Color color, Vector3 size, Transform parent, string sortingLayerName, int orderInLayer)
        {
            DrawAnchoredLeftBar(sprite, color, size, parent, null, sortingLayerName, orderInLayer, "FX Bar");
        }

        private static void DrawHealthBar(Sprite sprite, Color color, Vector3 size, Transform parent, Material material, string sortingLayerName, int orderInLayer)
        {
            DrawAnchoredLeftBar(sprite, color, size, parent, material, sortingLayerName, orderInLayer, "Health Bar");
        }

        private static void DrawAnchoredLeftBar(Sprite sprite, Color color, Vector3 size, Transform parent, Material material, string sortingLayerName, int orderInLayer, string name)
        {
            GameObject go = new GameObject(name);
            Transform barParentTransform = go.transform;
            barParentTransform.SetParent(parent, false);
            Transform barTransform = Utils.DrawSprite(sprite, color, size, Vector3.zero, barParentTransform, material, sortingLayerName, orderInLayer, "Bar Visual").transform;
            AdjustAnchoredPositions(barTransform, size, sprite);
        }

        public static void AdjustAnchoredPositions(Transform barTransform, Vector3 barSize, Sprite sprite)
        {
            float positionXCorrection = sprite.rect.width == 1f ? 1f : sprite.rect.width / 100f; // TODO Find out why...
            barTransform.localPosition = new Vector3(barSize.x / 2f * positionXCorrection, 0f, 0f);
            barTransform.parent.localPosition = -barTransform.localPosition;
        }
        #endregion
    }
#endif
}