using UnityEngine;
using UnityEngine.InputSystem;

public class MiningInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject rangeIndicatorPrefab;

    [Header("테스트용 범위")]
    [SerializeField] private float miningRange = 1f;

    [Header("광석 레이어")]
    [SerializeField] private LayerMask oreLayer;

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

        DetectOre(worldPos);
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
    private void DetectOre(Vector2 position)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                position,
                miningRange,
                oreLayer);

        Debug.Log($"감지된 광석 수 : {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"광석 감지 : {hit.name}");
        }
    }

}
