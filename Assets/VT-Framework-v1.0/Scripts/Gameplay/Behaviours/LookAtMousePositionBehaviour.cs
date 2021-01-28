using UnityEngine;
using VT.Utilities;

namespace VT.Gameplay.Behaviours
{
    public class LookAtMousePositionBehaviour : MonoBehaviour
    {
        [SerializeField] private bool flipX;

        private void Update()
        {
            LookAtMousePosition();
        }

        private void LookAtMousePosition()
        {
            Vector2 toMousePos = CursorManager.CursorPosition - transform.position;
            float angleZ = Mathf.Atan2(toMousePos.y, toMousePos.x) * Mathf.Rad2Deg;
            float angleX = 0;
            if (CursorManager.CursorPosition2D.x < transform.position.x && flipX)
            {
                angleX = 180;
                angleZ *= -1;
            }
            Quaternion targetRotation = Quaternion.Euler(angleX, 0, angleZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f);
        }
    }
}