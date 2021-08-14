using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VT.Utilities;

public class Test3DPoisson : MonoBehaviour
{
    public List<Vector3> PositionList => positionList;
    public Vector3 DimensionSize => dimensionSize;

    [SerializeField] private int nodeLimits;
    [SerializeField] private float separateRadius;
    [SerializeField] private Vector3 dimensionSize;
    [SerializeField] private List<Vector3> positionList = new List<Vector3>();
    [SerializeField] private float gizmosRadius;

    [Button]
    private void GeneratePosition()
    {
        CalculatePositionOffset();
        positionList = PoissonDiscSampling.GetSamples3D(separateRadius, dimensionSize, nodeLimits);
    }

    private Vector3 positionOffset;
    private float offsetX;
    private float offsetY;
    private float offsetZ;
    private Boundary3D boundary3D;

    private void Start()
    {
        CalculatePositionOffset();
    }
    
    private Vector3 lastPosition;
    private void OnDrawGizmosSelected()
    {
        if (lastPosition != transform.position)
        {
            lastPosition = transform.position;
            boundary3D = new Boundary3D(transform.position, dimensionSize);
            CalculatePositionOffset();
        }

        for (int i = 0; i < positionList.Count; i++)
        {
            Gizmos.DrawSphere(positionOffset + positionList[i], gizmosRadius);
        }

        ExtraGizmos.DrawBox(boundary3D);
    }

    private void CalculatePositionOffset()
    {
        offsetX = transform.position.x - dimensionSize.x / 2f;
        offsetY = transform.position.y - dimensionSize.y / 2f;
        offsetZ = transform.position.z - dimensionSize.z / 2f;
        positionOffset = new Vector3(offsetX, offsetY, offsetZ);
    }

    private Vector3 lastDimensionSize;
    private void OnValidate()
    {
        if (lastDimensionSize != dimensionSize)
        {
            lastDimensionSize = dimensionSize;
            boundary3D = new Boundary3D(transform.position, lastDimensionSize);
        }
    }
}
