using UnityEngine;

public class TestD : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"{TestMain.orderOfExecution++} {GetType().ToString()} Awake");
    }

    private void OnEnable()
    {
        Debug.Log($"{TestMain.orderOfExecution++} {GetType().ToString()} OnEnable");
    }

    private void Start()
    {
        Debug.Log($"{TestMain.orderOfExecution++} {GetType().ToString()} Start");
    }
}
