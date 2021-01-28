using System.Collections;
using UnityEngine;

namespace VT
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DragableComponent : MonoBehaviour
    {
        private static DragableComponent currentDraggedComponent = null;
        private bool isBeingHeld = false;
        private bool hasCalculatedOffsetFromCursor = false;
        private Vector2 offsetFromCursor = default;

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (!isBeingHeld) return;

            if (!hasCalculatedOffsetFromCursor)
            {
                offsetFromCursor = transform.position - CursorManager.CursorPosition;
                hasCalculatedOffsetFromCursor = true;
            }

            transform.position = CursorManager.CursorPosition2D + offsetFromCursor;
        }

        private void OnMouseOver()
        {
            if (currentDraggedComponent && currentDraggedComponent != this) return;

            if (CursorManager.IsLeftMouseBeingHeld)
            {
                isBeingHeld = true;
                currentDraggedComponent = this;
            }
            else
            {
                isBeingHeld = false;
                currentDraggedComponent = null;
                hasCalculatedOffsetFromCursor = false;
            }
        }
    }
}