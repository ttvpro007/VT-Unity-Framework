using UnityEngine;

namespace VT.Gameplay.HealthSystem
{
    public class HealthSystemTest : MonoBehaviour
    {
        private QuickWorldHealthBar quickWorldHealthBar;
        private QuickUIHealthBar quickUIHealthBar;

        private void Awake()
        {
            quickWorldHealthBar = GetComponentInChildren<QuickWorldHealthBar>();
            if (quickWorldHealthBar)
                quickWorldHealthBar.Setup(new Health());

            quickUIHealthBar = GetComponentInChildren<QuickUIHealthBar>();
            if (quickUIHealthBar)
                quickUIHealthBar.Setup(new Health());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                if (quickWorldHealthBar)
                {
                    SubtractHealth(quickWorldHealthBar.HealthSystem.MaxHealth / 10);
                }

                if (quickUIHealthBar)
                {
                    SubtractHealth(quickUIHealthBar.HealthSystem.MaxHealth / 10);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                if (quickWorldHealthBar)
                {
                    AddHealth(quickWorldHealthBar.HealthSystem.MaxHealth / 10);
                }

                if (quickUIHealthBar)
                {
                    AddHealth(quickUIHealthBar.HealthSystem.MaxHealth / 10);
                }
            }
        }

        private void AddHealth(float amount)
        {
            if (quickWorldHealthBar)
            {
                quickWorldHealthBar.HealthSystem.AddHealth(amount);
            }

            if (quickUIHealthBar)
            {
                quickUIHealthBar.HealthSystem.AddHealth(amount);
            }
        }

        private void SubtractHealth(float amount)
        {
            if (quickWorldHealthBar)
            {
                quickWorldHealthBar.HealthSystem.SubtractHealth(amount);
            }

            if (quickUIHealthBar)
            {
                quickUIHealthBar.HealthSystem.SubtractHealth(amount);
            }
        }
    }
}