using UnityEngine;

public static class CursorManager
{
    public static Vector3 CursorPosition { get => Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    public static Vector2 CursorPosition2D { get => CursorPosition; }
    public static bool IsLeftMouseBeingHeld { get => Input.GetMouseButton(0); }
    public static bool IsRightMouseBeingHeld { get => Input.GetMouseButton(1); }
}