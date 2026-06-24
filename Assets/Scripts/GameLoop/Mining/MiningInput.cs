using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiningInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RangeIndicator rangeIndicator;
    [SerializeField] private MiningSequence miningSequence;

    [Header("인디케이터 범위")]
    [SerializeField] private float miningRange = 1f;

    [Header("광석 레이어")]
    [SerializeField] private LayerMask oreLayer;
    Vector2 worldPos = Vector2.zero;
    Dictionary<StoneActor, float> lastHitTimeByStone = new();
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    private void OnEnable()
    { 
        rangeIndicator.transform.position = new Vector3(0, 1000f, 0);
    }
    private void Start()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopStarted, OnLoopStarted); 
    }

    public void OnLoopStarted()
    { 
    }
    private void Update()
    {
        if (!GameManager.Instance.GameLoop.IsRunning) return;
        worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        rangeIndicator.transform.position = worldPos; 
        rangeIndicator.Initialize(miningRange);

        DetectOre(worldPos);

    } 
    private void DetectOre(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, miningRange, oreLayer);

        Debug.Log($"감지된 광석 수 : {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            StoneActor stone = hit.GetComponentInParent<StoneActor>();
            if(CanHit(stone))
            { 
                miningSequence.RequestMine(stone);
                RecordHit(stone);
            }
        }
    }

    private bool CanHit(StoneActor stone)
    {
        if (!lastHitTimeByStone.TryGetValue(stone, out float lastTime))
            return true;

        float interval = 1f / GameManager.Instance.Upgrade.GetRuntimeStat().MiningSpeed;
        return Time.time - lastTime >= interval;
    }

    private void RecordHit(StoneActor stone)
    {
        lastHitTimeByStone[stone] = Time.time;
    }

}
