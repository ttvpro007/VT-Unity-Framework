using UnityEngine;

namespace VT.Utilities
{
    public struct Boundary3D
    {
        public Boundary3D(Vector3 center, Vector3 dimension)
        {
            xMin = center.x - dimension.x / 2f;
            xMax = center.x + dimension.x / 2f;
            yMin = center.y - dimension.y / 2f;
            yMax = center.y + dimension.y / 2f;
            zMin = center.z - dimension.z / 2f;
            zMax = center.z + dimension.z / 2f;
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

        public static bool operator ==(Boundary3D left, Boundary3D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Boundary3D left, Boundary3D right)
        {
            return !(left == right);
        }

        public float xMin, xMax, yMin, yMax, zMin, zMax;
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