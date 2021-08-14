using UnityEngine;

namespace VT.Utilities
{
    public static class CursorManager
    {
        public static Vector3 CursorPosition 
        {
            get
            {
                if (!mainCamera) 
                    mainCamera = Camera.main;

                return mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        public static Vector2 CursorPosition2D { get => CursorPosition; }
        public static bool IsLeftMouseClicked { get => Input.GetMouseButtonDown(0); }
        public static bool IsLeftMouseBeingHeld { get => Input.GetMouseButton(0); }
        public static bool IsLeftMouseReleased { get => Input.GetMouseButtonUp(0); }
        public static bool IsRightMouseClicked { get => Input.GetMouseButtonDown(1); }
        public static bool IsRightMouseBeingHeld { get => Input.GetMouseButton(1); }
        public static bool IsRightMouseReleased { get => Input.GetMouseButtonUp(1); }
        public static bool IsMouseWheelClicked { get => Input.GetMouseButtonDown(2); }
        public static bool IsMouseWheelBeingHeld { get => Input.GetMouseButton(2); }
        public static bool IsMouseWheelReleased { get => Input.GetMouseButtonUp(2); }

        private static Camera mainCamera = Camera.main;
    }
}