using UnityEngine;

namespace VT.Utilities
{
#if UNITY_EDITOR
    public class Boundary2DGizmos : MonoBehaviour
    {
        [SerializeField] private Vector2 dimension = default;
        [SerializeField] private float screenWidth = default;
        [SerializeField] private float screenHeight = default;
        [SerializeField] private Vector2 rectDimension = default;

        private float conversion = default;

        private Boundary2D boundary2D;

        public void SetDimension(Vector2 dimension)
        {
            this.dimension = dimension;
            CalculateRectDimension();
            SetRectTransformSideDelta();
        }

        private Vector3 lastPosition;
        private Boundary2D lastBoundary2D;
        private void OnDrawGizmos()
        {
            if (lastPosition != transform.position)
            {
                lastPosition = transform.position;
                boundary2D = new Boundary2D(lastPosition, dimension);
            }

            Gizmos.color = Color.red;
            ExtraGizmos.DrawRect(boundary2D);
        }

        private Vector2 lastDimension;
        private RectTransform rectTransform = null;
        private void OnValidate()
        {
            CalculateRectDimension();

            if (lastDimension != dimension)
            {
                lastDimension = dimension;
                boundary2D = new Boundary2D(transform.position, lastDimension);
            }
        }

        private void SetRectTransformSideDelta()
        {
            if (!rectTransform)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            else
            {
                rectTransform.sizeDelta = rectDimension;
            }
        }

        private void CalculateRectDimension()
        {
            if (screenHeight != 0f)
            {
                conversion = 10f / screenHeight;
            }

            if (conversion != 0)
            {
                rectDimension = dimension / conversion;
            }
        }
    }
#endif
}