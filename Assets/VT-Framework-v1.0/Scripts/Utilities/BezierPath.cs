using UnityEngine;

namespace VT
{
    public static class BezierPath
    {
        public static Vector3[] CreatePath(Vector3 start, Vector3 end, Vector3 bezier1, Vector3 bezier2, int segments = 10)
        {
            Vector3[] points = new Vector3[segments + 1];
            float step = 1f / (segments + 1);
            float t = 0f;

            for (int i = 0; i < segments + 1; i++)
            {
                t = step * i;

                points[i] = Mathf.Pow(1 - t, 3) * start +
                            3 * Mathf.Pow(1 - t, 2) * t * bezier1 +
                            3 * (1 - t) * Mathf.Pow(t, 2) * bezier2 +
                            3 * Mathf.Pow(t, 3) * end;
            }

            return points;
        }
    }
}