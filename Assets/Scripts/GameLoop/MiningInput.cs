using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiningInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RangeIndicator rangeIndicator;

    [Header("인디케이터 범위")]
    [SerializeField] private float miningRange = 1f;

    [Header("광석 레이어")]
    [SerializeField] private LayerMask oreLayer;
    Vector2 worldPos = Vector2.zero;
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Start()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
        StopCoroutine(CallMethodNTimes());
    }

    public void OnLoopStarted()
    {
        StartCoroutine(CallMethodNTimes());
    }
    private void Update()
    {
        if (!GameManager.Instance.GameLoop.IsRunning) return;
        worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        rangeIndicator.transform.position = worldPos; 
        rangeIndicator.Initialize(miningRange);
         
    }
     

    IEnumerator CallMethodNTimes()
    {
        // 1초에 n번이므로 각 호출 간격은 1 / n 초

        while (true)
        {
            float interval = 1f / GameManager.Instance.Upgrade.GetRuntimeStat().MiningSpeed;
            DetectOre(worldPos);
            
            yield return new WaitForSeconds(interval);
        }
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
            hit.GetComponentInParent<HPHandler>().TakeDamage(
                GameManager.Instance.GameLoop.MiningCalculator.CalculateDamage()
                );
        }
    }

}
