using UnityEngine;

namespace VT.Utilities
{
    public struct Boundary2D
    {
        public Boundary2D(Vector2 center, Vector2 dimension)
        {
            Center = center;
            Dimension = dimension;
            xMin = Center.x - Dimension.x / 2f;
            xMax = Center.x + Dimension.x / 2f;
            yMin = Center.y - Dimension.y / 2f;
            yMax = Center.y + Dimension.y / 2f;
            Top = new Vector2(Center.x, yMax);
            Bottom = new Vector2(Center.x, yMin);
            Left = new Vector2(xMin, Center.y);
            Right = new Vector2(xMax, Center.y);
            TopLeft = new Vector2(xMin, yMax);
            TopRight = new Vector2(xMax, yMax);
            BottomLeft = new Vector2(xMin, yMin);
            BottomRight = new Vector2(xMax, yMin);
        }

        public bool Contains(Vector2 position)
        {
            return position.x >= xMin
                && position.x <= xMax
                && position.y >= yMin
                && position.y <= yMax;
        }

        public Vector2 Clamp(Vector2 position)
        {
            float x = Mathf.Clamp(position.x, xMin, xMax);
            float y = Mathf.Clamp(position.y, yMin, yMax);
            return new Vector2(x, y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + xMin.GetHashCode();
                hash = hash * 23 + xMax.GetHashCode();
                hash = hash * 23 + yMin.GetHashCode();
                hash = hash * 23 + yMax.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is Boundary2D)) return false;

            return Equals((Boundary2D)other);
        }

        public bool Equals(Boundary2D other)
        {
            return xMin == other.xMin
                && xMax == other.xMax
                && yMin == other.yMin
                && yMax == other.yMax;
        }

        public static Boundary2D Zero => new Boundary2D();

        public static Boundary2D operator *(Boundary2D left, float right) => new Boundary2D(left.Center, left.Dimension * right);

        public static Boundary2D operator *(float left, Boundary2D right) => new Boundary2D(right.Center, right.Dimension * left);

        public static Boundary2D operator /(Boundary2D left, float right) => new Boundary2D(left.Center, left.Dimension / right);

        public static Boundary2D operator /(float left, Boundary2D right) => new Boundary2D(right.Center, right.Dimension / left);

        public static bool operator ==(Boundary2D left, Boundary2D right) => left.Equals(right);

        public static bool operator !=(Boundary2D left, Boundary2D right) => !(left == right);

        public float xMin, xMax, yMin, yMax;
        public Vector2 Dimension;
        public Vector2 Center;
        public Vector2 Top;
        public Vector2 Bottom;
        public Vector2 Left;
        public Vector2 Right;
        public Vector2 TopLeft;
        public Vector2 TopRight;
        public Vector2 BottomLeft;
        public Vector2 BottomRight;
    }
}