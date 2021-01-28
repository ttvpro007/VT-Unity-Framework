using UnityEngine;

namespace VT.Utilities
{
    public class ExtraGizmos : MonoBehaviour
    {
        public static void DrawBox(Boundary3D boundary3D)
        {
            // front
            DrawRect(boundary3D.TopLeftFront, boundary3D.TopRightFront, boundary3D.BottomRightFront, boundary3D.BottomLeftFront);

            // back
            DrawRect(boundary3D.TopLeftBack, boundary3D.TopRightBack, boundary3D.BottomRightBack, boundary3D.BottomLeftBack);

            // connect front back
            Gizmos.DrawLine(boundary3D.TopLeftFront, boundary3D.TopLeftBack);
            Gizmos.DrawLine(boundary3D.TopRightFront, boundary3D.TopRightBack);
            Gizmos.DrawLine(boundary3D.BottomRightFront, boundary3D.BottomRightBack);
            Gizmos.DrawLine(boundary3D.BottomLeftFront, boundary3D.BottomLeftBack);
        }

        public static void DrawRect(Boundary2D boundary2D)
        {
            DrawRect(boundary2D.TopLeft, boundary2D.TopRight, boundary2D.BottomRight, boundary2D.BottomLeft);
        }

        public static void DrawRect(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft)
        {
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}