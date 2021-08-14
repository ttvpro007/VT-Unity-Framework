using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VT.QuickBar;
using VT.Extensions;
using VT.Gameplay.HealthSystem.Enums;
using VT.Gameplay.HealthSystem.ScriptableObjects;
using VT.Utilities.Factory;
using VT.Utilities;

namespace VT.Gameplay.HealthSystem.Editors
{
    public class QuickHealthBarEditorWindow : OdinEditorWindow
    {
        [MenuItem("VT Framework/Quick World Health Bar Editor")]
        private static void OpenWindow()
        {
            QuickHealthBarEditorWindow window = GetWindow<QuickHealthBarEditorWindow>();
            window.minSize = new Vector2(450, 250);
            window.maxSize = new Vector2(450, 550);
            window.Show();
        }

        [OnValueChanged("SetDirty")]
        [SerializeField, HideLabel] private QuickHealthBarPresets quickHealthBarPresets;

        [SerializeField]
        [TitleGroup("Presets Settings")]
        [LabelText("Presets")]
        [OnValueChanged("LoadPresets")]
        [InlineButton("OverridePresets", Label = "Override", ShowIf = "@isDirty && quickHealthBarPresetsSO")]
        [InlineButton("UnloadPresets", Label = "Unload", ShowIf = "@quickHealthBarPresetsSO != null")]
        [InlineButton("CreatePresets", Label = "Save Current", ShowIf = "@quickHealthBarPresetsSO == null")]
        private QuickHealthBarPresetsSO quickHealthBarPresetsSO;

        private GameObject quickHealthBarGO;
        private readonly string defaultQuickWorldHealthBarAssetPath = "Default Quick World Health Bar Presets";
        private readonly string defaultQuickUIHealthBarAssetPath = "Default Quick UI Health Bar Presets";
        private readonly string defaultRelativeAssetsPath = "Assets/Quick Health Bar Presets";
        private bool isDirty = false;

        private new void SetDirty()
        {
            isDirty = true;
        }

        private void ClearDirty()
        {
            isDirty = false; 
        }

        private void OverridePresets()
        {
            if (quickHealthBarPresetsSO)
            {
                if (EditorUtility.DisplayDialog("Warning", $"Overriding [{quickHealthBarPresetsSO.name}]. Are you sure?", "Yes", "No"))
                {
                    Undo.RecordObject(quickHealthBarPresetsSO, $"Override {quickHealthBarPresetsSO.name}");
                    quickHealthBarPresetsSO.SetPresets(quickHealthBarPresets);
                    ClearDirty();
                }
            }
        }

        private void UnloadPresets()
        {
            quickHealthBarPresetsSO = null;
            ClearDirty();
        }

        private void LoadPresets()
        {
            if (quickHealthBarPresetsSO && quickHealthBarPresetsSO.QuickHealthBarPresets != quickHealthBarPresets)
                quickHealthBarPresets = quickHealthBarPresetsSO.QuickHealthBarPresets;

            ClearDirty();
        }

        private void CreatePresets()
        {
            quickHealthBarPresetsSO = VTObjectFactory.CreateScriptableObjectAsset<QuickHealthBarPresetsSO>("QuickHealthBarPresetsSO", defaultRelativeAssetsPath, true);

            if (quickHealthBarPresetsSO)
            {
                quickHealthBarPresetsSO.SetPresets(quickHealthBarPresets);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!quickHealthBarPresetsSO)
            {
                LoadDefaultPresets();
            }
            else
            {
                LoadPresets();
            }
        }

        private string CreateHealthBarButtonName()
        {
            return Selection.activeGameObject ? $"Create Health Bar for {Selection.activeGameObject.name}" : "Create Health Bar";
        }

        [Button("@CreateHealthBarButtonName()")]
        private void CreateHealthBar()
        {
            switch (quickHealthBarPresets.Type)
            {
                case HealthBarType.World:
                    CreateQuickWorldHealthBar();
                    break;
                case HealthBarType.UI:
                    CreateQuickUIHealthBar();
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            quickHealthBarGO = null;
        }

        private string LoadDefaultSettingsButtonName()
        {
            switch (quickHealthBarPresets.Type)
            {
                case HealthBarType.World:
                    return "Load Default World Health Bar Settings";
                case HealthBarType.UI:
                    return "Load Default UI Health Bar Settings";
                default:
                    throw new System.NotImplementedException();
            }
        }

        [Button("@LoadDefaultSettingsButtonName()")]
        private void LoadDefaultPresets()
        {
            ClearDirty();

            switch (quickHealthBarPresets.Type)
            {
                case HealthBarType.World:
                    quickHealthBarPresetsSO = LoadDefaultQuickWorldHealthBarPresetsSO();
                    quickHealthBarPresets = quickHealthBarPresetsSO.QuickHealthBarPresets;
                    break;
                case HealthBarType.UI:
                    quickHealthBarPresetsSO = LoadDefaultQuickUIHealthBarPresetsSO();
                    quickHealthBarPresets = quickHealthBarPresetsSO.QuickHealthBarPresets;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        private QuickHealthBarPresetsSO LoadDefaultQuickWorldHealthBarPresetsSO()
        {
            return Resources.Load<QuickHealthBarPresetsSO>(defaultQuickWorldHealthBarAssetPath);
        }

        private QuickHealthBarPresetsSO LoadDefaultQuickUIHealthBarPresetsSO()
        {
            return Resources.Load<QuickHealthBarPresetsSO>(defaultQuickUIHealthBarAssetPath);
        }

        private void CreateQuickWorldHealthBar()
        {
            QuickWorldHealthBar quickWorldHealthBar = VTObjectFactory.CreateEditorGameObject<QuickWorldHealthBar>("Quick Health Bar", typeof(QuickWorldHealthBar));
            quickWorldHealthBar.gameObject.SetParentAndAlign(Selection.activeGameObject).Focus();
            quickWorldHealthBar.SetPresets(quickHealthBarPresets);

            Sprite defaultSprite = Resources.Load<Sprite>("White_1x1");
            QuickBarSprites sprites = new QuickBarSprites
            (
                quickHealthBarPresets.OutlineSprite ? quickHealthBarPresets.OutlineSprite : defaultSprite,
                quickHealthBarPresets.OutlineSprite ? quickHealthBarPresets.BackgroundSprite : defaultSprite,
                quickHealthBarPresets.OutlineSprite ? quickHealthBarPresets.FXBarSprite : defaultSprite,
                quickHealthBarPresets.OutlineSprite ? quickHealthBarPresets.HealthBarSprite : defaultSprite
            );

            quickHealthBarGO = QuickWorldBar.Create
            (
                sprites,
                new QuickBarColors
                (
                    quickHealthBarPresets.OutlineColor,
                    quickHealthBarPresets.BackgroundColor,
                    quickHealthBarPresets.FXBarColor,
                    quickHealthBarPresets.UseHealthBarMaterial && quickHealthBarPresets.HealthBarMaterial ? Color.white : quickHealthBarPresets.HealthBarColor
                ),
                quickHealthBarPresets.Size,
                quickHealthBarPresets.PositionOffset,
                quickWorldHealthBar.transform,
                quickHealthBarPresets.UseHealthBarMaterial ? quickHealthBarPresets.HealthBarMaterial : null,
                quickHealthBarPresets.OutlineThickness,
                quickHealthBarPresets.SortingLayer.ToString(),
                quickHealthBarPresets.OrderInLayer
            );

            if (quickHealthBarPresets.IncludeNumber)
            {
                SetupWorldTMP(quickWorldHealthBar.gameObject);
            }
        }

        private void CreateQuickUIHealthBar()
        {
            QuickUIHealthBar quickUIHealthBar = VTObjectFactory.CreateEditorGameObject<QuickUIHealthBar>("Quick Health Bar", typeof(RectTransform), typeof(QuickUIHealthBar));
            quickUIHealthBar.gameObject.SetParentAndAlign(Selection.activeGameObject).Focus();
            quickUIHealthBar.SetPresets(quickHealthBarPresets);

            if (!quickUIHealthBar.transform.parent)
            {
                quickUIHealthBar.gameObject.SetParentAndAlign(VTObjectFactory.CreateSceneCanvas());
            }

            QuickBarSprites sprites = new QuickBarSprites
            (
                quickHealthBarPresets.OutlineSprite,
                quickHealthBarPresets.BackgroundSprite,
                quickHealthBarPresets.FXBarSprite,
                quickHealthBarPresets.HealthBarSprite
            );

            quickHealthBarGO = QuickUIBar.Create
            (
                sprites,
                new QuickBarColors
                (
                    quickHealthBarPresets.OutlineColor,
                    quickHealthBarPresets.BackgroundColor,
                    quickHealthBarPresets.FXBarColor,
                    quickHealthBarPresets.UseHealthBarMaterial && quickHealthBarPresets.HealthBarMaterial ? Color.white : quickHealthBarPresets.HealthBarColor
                ),
                quickHealthBarPresets.Size,
                quickHealthBarPresets.PositionOffset,
                quickUIHealthBar.GetComponent<RectTransform>(),
                quickHealthBarPresets.UseHealthBarMaterial ? quickHealthBarPresets.HealthBarMaterial : null,
                quickHealthBarPresets.OutlineThickness
            );

            if (quickHealthBarPresets.IncludeNumber)
            {
                SetupUITMP(quickUIHealthBar.gameObject);
            }
        }

        private void SetupWorldTMP(GameObject parent)
        {
            TextMeshPro tmpComponent = VTObjectFactory.CreateEditorGameObject<TextMeshPro>("Health Text");
            tmpComponent.gameObject.SetParentAndAlign(parent);
            tmpComponent.renderer.sortingLayerName = quickHealthBarPresets.SortingLayer.ToString();

            if (quickHealthBarGO)
            {
                tmpComponent.sortingOrder = quickHealthBarGO.transform
                    .GetFirstComponentOfTypeInHierachy<Transform>(t => t.name == "Health Bar")
                    .GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
            }
            else
            {
                tmpComponent.sortingOrder = quickHealthBarPresets.OrderInLayer;
            }

            SetupTMP(tmpComponent);
        }

        private void SetupUITMP(GameObject parent)
        {
            TextMeshProUGUI tmpComponent = VTObjectFactory.CreateEditorGameObject<TextMeshProUGUI>("Health Text");
            tmpComponent.gameObject.SetParentAndAlign(parent);
            SetupTMP(tmpComponent);
        }

        private void SetupTMP(TMP_Text tmpComponent)
        {
            if (quickHealthBarPresets.FontAsset)
            {
                tmpComponent.font = quickHealthBarPresets.FontAsset;
            }

            tmpComponent.text = "100/100";
            tmpComponent.alignment = TextAlignmentOptions.Center;
            tmpComponent.enableWordWrapping = false;
            tmpComponent.fontStyle = quickHealthBarPresets.FontStyle;
            tmpComponent.enableAutoSizing = true;
            tmpComponent.fontSizeMin = quickHealthBarPresets.MinFontSize;
            tmpComponent.fontSizeMax = quickHealthBarPresets.MaxFontSize;
            tmpComponent.enableWordWrapping = true;
            tmpComponent.rectTransform.sizeDelta = quickHealthBarPresets.Size * 0.9f;
            tmpComponent.rectTransform.localPosition = quickHealthBarPresets.PositionOffset;
        }
    }
}