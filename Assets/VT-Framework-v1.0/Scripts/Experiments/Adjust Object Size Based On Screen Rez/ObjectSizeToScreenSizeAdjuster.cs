using Sirenix.OdinInspector;
using UnityEngine;
using VT.Diagnostics;

public class ObjectSizeToScreenSizeAdjuster : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [Button]
    private void RescaleGameObjectXZ()
    {
        Benchmarker.Benchmark(() =>
        {
            transform.localScale = new Vector3(transform.localScale.z * mainCamera.aspect, transform.localScale.y, transform.localScale.z);
        }, 1,
        "Using Drag and Drop reference (Camera)");
        Benchmarker.Benchmark(() =>
        {
            transform.localScale = new Vector3(transform.localScale.z * Camera.main.aspect, transform.localScale.y, transform.localScale.z);
        }, 1,
        "Using Camera.main reference (Camera)");
    }
}
