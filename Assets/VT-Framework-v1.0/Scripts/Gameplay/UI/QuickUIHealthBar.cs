using UnityEngine;
using VT.Utilities;

namespace VT.Gameplay.UI
{
    public class QuickUIHealthBar : QuickHealthBar
    {
        private QuickUIBar quickUIHealthBar = null;

        protected override void Awake()
        {
            base.Awake();
            
            quickUIHealthBar = new QuickUIBar(new QuickBar.Colors(outlineColor, backgroundColor, foregroundColor), size, positionOffset, transform.GetComponent<RectTransform>());
        }

        protected override void OnHealthChangedHandler()
        {
            base.OnHealthChangedHandler();

            if (quickUIHealthBar != null)
            {
                quickUIHealthBar.SetSize(health.HealthPercentage);
            }
        }
    }
}