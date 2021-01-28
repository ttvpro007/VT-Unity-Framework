using VT.Utilities;

namespace VT.Gameplay.UI
{
    public class QuickWorldHealthBar : QuickHealthBar
    {
        private QuickWorldBar quickWorldHealthBar = null;

        protected override void Awake()
        {
            base.Awake();

            quickWorldHealthBar = new QuickWorldBar(blankSprite, new QuickBar.Colors(outlineColor, backgroundColor, foregroundColor), size, positionOffset, transform, outlineThickness);
        }

        protected override void OnHealthChangedHandler()
        {
            base.OnHealthChangedHandler();
            
            if (quickWorldHealthBar != null)
            {
                quickWorldHealthBar.SetSize(health.HealthPercentage);
            }
        }
    }
}