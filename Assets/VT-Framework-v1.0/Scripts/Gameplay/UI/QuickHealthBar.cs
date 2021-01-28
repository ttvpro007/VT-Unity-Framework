using UnityEngine;
using VT.Gameplay.Core;

namespace VT.Gameplay.UI
{
    public abstract class QuickHealthBar : MonoBehaviour
    {
        [SerializeField] protected bool showNumber = default;
        [SerializeField] protected int startHealth = 100;
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected Sprite blankSprite = default;
        [SerializeField] protected Color outlineColor = Color.black;
        [SerializeField] protected Color backgroundColor = Color.red;
        [SerializeField] protected Color foregroundColor = Color.green;
        [SerializeField] protected float outlineThickness = 0.1f;
        [SerializeField] protected Vector3 size = Vector3.one;
        [SerializeField] protected Vector3 positionOffset = default;

        protected Health health;

        protected virtual void Awake()
        {
            health = new Health(startHealth, maxHealth);
            health.OnHealthChanged += OnHealthChangedHandler;
        }

        protected virtual void OnHealthChangedHandler()
        {
            if (showNumber)
            {
                Debug.Log(health.LastHealthModifierValue);
            }
        }
    }
}