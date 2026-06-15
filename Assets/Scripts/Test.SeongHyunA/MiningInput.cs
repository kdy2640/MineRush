using UnityEngine;
using UnityEngine.InputSystem;

public class MiningInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject rangeIndicatorPrefab;

    [Header("테스트용 범위")]
    [SerializeField] private float miningRange = 1f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 worldPos =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());

        ShowRange(worldPos);
    }

    private void ShowRange(Vector2 position)
    {
        GameObject obj = Instantiate(
            rangeIndicatorPrefab,
            position,
            Quaternion.identity);

        RangeIndicator indicator =
            obj.GetComponent<RangeIndicator>();

        indicator.Initialize(miningRange);
    }

}
