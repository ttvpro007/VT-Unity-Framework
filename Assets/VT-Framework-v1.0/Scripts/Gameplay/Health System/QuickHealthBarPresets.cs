using Sirenix.OdinInspector;
using UnityEngine;

namespace VT.Gameplay.HealthSystem
{
    [System.Serializable]
    public struct QuickHealthBarPresets
    {
        [EnumPaging]
        public Enums.HealthBarType Type;
        [ShowIf("Type", Enums.HealthBarType.World)]
        public bool AlwaysShow;
        [ToggleGroup("IncludeNumber", "Include Number")]
        public bool IncludeNumber;
        [EnumToggleButtons, HideLabel, BoxGroup("IncludeNumber/Font Visuals")]
        public TMPro.FontStyles FontStyle;
        [BoxGroup("IncludeNumber/Font Visuals")]
        public TMPro.TMP_FontAsset FontAsset;
        [BoxGroup("IncludeNumber/Font Size"), Min(0.1f)]
        public float MinFontSize;
        [BoxGroup("IncludeNumber/Font Size"), Min(0.1f)]
        public float MaxFontSize;

        [ToggleGroup("UseHealthBarMaterial", "Use Health Bar Material")]
        public bool UseHealthBarMaterial;
        [ToggleGroup("UseHealthBarMaterial"), InfoBox("Foreground color is disregared when material is used.")]
        public Material HealthBarMaterial;

        [LabelWidth(120f)]
        [PreviewField(50f)]
        [TitleGroup("Visual Settings")]
        [HorizontalGroup("Visual Settings/Split")]
        [VerticalGroup("Visual Settings/Split/Left")]
        [BoxGroup("Visual Settings/Split/Left/Sprites")]
        public Sprite OutlineSprite, BackgroundSprite, FXBarSprite, HealthBarSprite;

        [LabelWidth(120f)]
        [VerticalGroup("Visual Settings/Split/Right")]
        [BoxGroup("Visual Settings/Split/Right/Colors")]
        public Color OutlineColor, BackgroundColor, FXBarColor, HealthBarColor;

        [Min(0f)]
        [LabelWidth(120f)]
        [BoxGroup("Visual Settings/Split/Right/Extras")]
        public float OutlineThickness;

        [LabelWidth(120f)]
        [BoxGroup("Visual Settings/Split/Right/Extras")]
        public Vector3 Size, PositionOffset;

        [LabelWidth(120f)]
        [BoxGroup("Visual Settings/Split/Right/Extras")]
        [ShowIf("Type", Enums.HealthBarType.World), EnumPaging]
        public Utilities.Enums.SortingLayer SortingLayer;

        [LabelWidth(120f)]
        [ShowIf("Type", Enums.HealthBarType.World)]
        [BoxGroup("Visual Settings/Split/Right/Extras")]
        public int OrderInLayer;

        [EnumToggleButtons, ShowIf("Type", Enums.HealthBarType.World)]
        public FXModules.Enums.EWorldFXModule WorldDamageFX;
        [EnumToggleButtons, ShowIf("Type", Enums.HealthBarType.UI)]
        public FXModules.Enums.EUIFXModule UIDamageFX;

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash ^= 23 + Type.GetHashCode();
                hash ^= 23 + AlwaysShow.GetHashCode();
                hash ^= 23 + IncludeNumber.GetHashCode();
                hash ^= 23 + FontStyle.GetHashCode();
                hash ^= 23 + FontAsset.GetHashCode();
                hash ^= 23 + MinFontSize.GetHashCode();
                hash ^= 23 + MaxFontSize.GetHashCode();
                hash ^= 23 + UseHealthBarMaterial.GetHashCode();
                hash ^= 23 + HealthBarMaterial.GetHashCode();
                hash ^= 23 + OutlineSprite.GetHashCode();
                hash ^= 23 + BackgroundSprite.GetHashCode();
                hash ^= 23 + FXBarSprite.GetHashCode();
                hash ^= 23 + HealthBarSprite.GetHashCode();
                hash ^= 23 + OutlineColor.GetHashCode();
                hash ^= 23 + BackgroundColor.GetHashCode();
                hash ^= 23 + FXBarColor.GetHashCode();
                hash ^= 23 + HealthBarColor.GetHashCode();
                hash ^= 23 + OutlineThickness.GetHashCode();
                hash ^= 23 + PositionOffset.GetHashCode();
                hash ^= 23 + SortingLayer.GetHashCode();
                hash ^= 23 + OrderInLayer.GetHashCode();
                hash ^= 23 + WorldDamageFX.GetHashCode();
                hash ^= 23 + UIDamageFX.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is QuickHealthBarPresets)) return false;

            return Equals((QuickHealthBarPresets)other);
        }

        public bool Equals(QuickHealthBarPresets other)
        {
            return Type == other.Type
            && AlwaysShow == other.AlwaysShow
            && IncludeNumber == other.IncludeNumber
            && FontStyle == other.FontStyle
            && FontAsset == other.FontAsset
            && MinFontSize == other.MinFontSize
            && MaxFontSize == other.MaxFontSize
            && UseHealthBarMaterial == other.UseHealthBarMaterial
            && HealthBarMaterial == other.HealthBarMaterial
            && OutlineSprite == other.OutlineSprite
            && BackgroundSprite == other.BackgroundSprite
            && FXBarSprite == other.FXBarSprite
            && HealthBarSprite== other.HealthBarSprite
            && OutlineColor == other.OutlineColor
            && BackgroundColor == other.BackgroundColor
            && FXBarColor == other.FXBarColor
            && HealthBarColor == other.HealthBarColor
            && OutlineThickness == other.OutlineThickness
            && PositionOffset == other.PositionOffset
            && SortingLayer == other.SortingLayer
            && OrderInLayer == other.OrderInLayer
            && WorldDamageFX == other.WorldDamageFX
            && UIDamageFX == other.UIDamageFX;
        }

        public static QuickHealthBarPresets Zero => new QuickHealthBarPresets();

        public static bool operator ==(QuickHealthBarPresets left, QuickHealthBarPresets right) => left.Equals(right);

        public static bool operator !=(QuickHealthBarPresets left, QuickHealthBarPresets right) => !(left == right);
    }
}