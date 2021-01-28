using UnityEngine;

namespace VT
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GenericDragableCompoment2D : MonoBehaviour
    {
        private static GenericDragableCompoment2D currentDraggedComponent = null;
        private bool isBeingHeld = false;
        private bool hasCalculatedOffsetFromCursor = false;
        private Vector2 offsetFromCursor = default;
        private Vector3 cursorPosition = default;
        private Vector2 cursorPosition2D = default;

        private void Update()
        {
            MoveToCursorPositionWithOffset();
        }

        private void MoveToCursorPositionWithOffset()
        {
            if (!isBeingHeld) return;

            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition2D = cursorPosition;

            if (!hasCalculatedOffsetFromCursor)
            {
                offsetFromCursor = transform.position - cursorPosition;
                hasCalculatedOffsetFromCursor = true;
            }

            transform.position = cursorPosition2D + offsetFromCursor;
        }

        private void OnMouseOver()
        {
            if (currentDraggedComponent && currentDraggedComponent != this) return;

            if (Input.GetMouseButtonDown(0))
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