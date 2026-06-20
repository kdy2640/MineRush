using UnityEngine;
using UnityEngine.InputSystem;

public class RangeIndicatorUI : MonoBehaviour
{
    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    private void Update()
    {
        if (targetCamera == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 mousePos =
            targetCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());

        transform.position = mousePos;
    }
}