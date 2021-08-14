using UnityEngine;

namespace VT.Utilities
{
    public struct Boundary3D
    {
        public Boundary3D(Vector3 center, Vector3 dimension)
        {
            Center = center;
            Dimension = dimension;
            xMin = Center.x - Dimension.x / 2f;
            xMax = Center.x + Dimension.x / 2f;
            yMin = Center.y - Dimension.y / 2f;
            yMax = Center.y + Dimension.y / 2f;
            zMin = Center.z - Dimension.z / 2f;
            zMax = Center.z + Dimension.z / 2f;
            Top = new Vector3(Center.x, yMax, Center.z);
            Bottom = new Vector3(Center.x, yMin, Center.z);
            Left = new Vector3(xMin, Center.y, Center.z);
            Right = new Vector3(xMax, Center.y, Center.z);
            Back = new Vector3(Center.x, Center.y, zMin);
            Front = new Vector3(Center.x, Center.y, zMax);
            TopLeftFront = new Vector3(xMin, yMax, zMax);
            TopRightFront = new Vector3(xMax, yMax, zMax);
            BottomLeftFront = new Vector3(xMin, yMin, zMax);
            BottomRightFront = new Vector3(xMax, yMin, zMax);
            TopLeftBack = new Vector3(xMin, yMax, zMin);
            TopRightBack = new Vector3(xMax, yMax, zMin);
            BottomLeftBack = new Vector3(xMin, yMin, zMin);
            BottomRightBack = new Vector3(xMax, yMin, zMin);
        }

        public bool Contains(Vector3 position)
        {
            return position.x >= xMin
                && position.x <= xMax
                && position.y >= yMin
                && position.y <= yMax
                && position.z >= zMin
                && position.z <= zMax;
        }

        public Vector3 Clamp(Vector3 position)
        {
            float x = Mathf.Clamp(position.x, xMin, xMax);
            float y = Mathf.Clamp(position.y, yMin, yMax);
            float z = Mathf.Clamp(position.z, zMin, zMax);
            return new Vector3(x, y, z);
        }

        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode/263416
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + xMin.GetHashCode();
                hash = hash * 23 + xMax.GetHashCode();
                hash = hash * 23 + yMin.GetHashCode();
                hash = hash * 23 + yMax.GetHashCode();
                hash = hash * 23 + zMin.GetHashCode();
                hash = hash * 23 + zMax.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is Boundary3D)) return false;

            return Equals((Boundary3D)other);
        }

        public bool Equals(Boundary3D other)
        {
            return xMin == other.xMin
                && xMax == other.xMax
                && yMin == other.yMin
                && yMax == other.yMax
                && zMin == other.zMin
                && zMax == other.zMax;
        }

        public static Boundary3D operator *(Boundary3D left, float right) => new Boundary3D(left.Center, left.Dimension * right);

        public static Boundary3D operator *(float left, Boundary3D right) => new Boundary3D(right.Center, right.Dimension * left);

        public static Boundary3D operator /(Boundary3D left, float right) => new Boundary3D(left.Center, left.Dimension / right);

        public static Boundary3D operator /(float left, Boundary3D right) => new Boundary3D(right.Center, right.Dimension / left);

        public static bool operator ==(Boundary3D left, Boundary3D right) => left.Equals(right);

        public static bool operator !=(Boundary3D left, Boundary3D right) => !(left == right);

        public float xMin, xMax, yMin, yMax, zMin, zMax;
        public Vector3 Dimension;
        public Vector3 Center;
        public Vector3 Top;
        public Vector3 Bottom;
        public Vector3 Left;
        public Vector3 Right;
        public Vector3 Back;
        public Vector3 Front;
        public Vector3 TopLeftFront;
        public Vector3 TopRightFront;
        public Vector3 BottomLeftFront;
        public Vector3 BottomRightFront;
        public Vector3 TopLeftBack;
        public Vector3 TopRightBack;
        public Vector3 BottomLeftBack;
        public Vector3 BottomRightBack;
    }
}