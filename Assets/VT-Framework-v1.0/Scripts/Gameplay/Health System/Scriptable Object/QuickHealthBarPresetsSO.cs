using UnityEngine;

namespace VT.Gameplay.HealthSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Quick Health Bar Presets SO", menuName = "VT/Health System/Create Quick Health Bar Presets SO")]
    public class QuickHealthBarPresetsSO : ScriptableObject
    {
        [SerializeField, Sirenix.OdinInspector.HideLabel] private QuickHealthBarPresets quickHealthBarPresets;

        public QuickHealthBarPresets QuickHealthBarPresets => quickHealthBarPresets;

        public void SetPresets(QuickHealthBarPresets quickHealthBarPresets)
        {
            this.quickHealthBarPresets = quickHealthBarPresets;
        }
    }
}