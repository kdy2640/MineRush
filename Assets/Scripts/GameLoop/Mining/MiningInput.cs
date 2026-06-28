 using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MiningInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RangeIndicator rangeIndicator;
    [SerializeField] private MiningSequence miningSequence;
    [SerializeField] private StoneSpawner spawner;
    [Header("인디케이터 기본 범위")]
    [SerializeField] private float miningRange = 0.75f;

    [Header("광석 레이어")]
    [SerializeField] private LayerMask oreLayer;
    Vector2 worldPos = Vector2.zero;
    private float pickaxelastTime = 0f;

    private float lastSkillTime = 0f;
    public float GetMiningRange()
    {
        if (!Application.isPlaying)
            return miningRange;
        return miningRange * GameManager.Instance.Upgrade.GetRuntimeStat().MiningRadius; 
    } 
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
        rangeIndicator.gameObject.SetActive(false);
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopEnded, OnLoopEnded); 
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopEnded, OnLoopEnded);
    }


    public void OnLoopStarted()
    {
        rangeIndicator.gameObject.SetActive(true);
    }
    public void OnLoopEnded()
    {
        rangeIndicator.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!GameManager.Instance.GameLoop.IsRunning) return;
        worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        rangeIndicator.transform.position = worldPos; 
        rangeIndicator.Initialize(GetMiningRange());

        float interval = 1f / GameManager.Instance.Upgrade.GetRuntimeStat().MiningSpeed;
        if (Time.time - pickaxelastTime >= interval)
        {
            ResolveTargetMining(worldPos,MiningType.Pickaxe);
            pickaxelastTime = Time.time;
        } 

        if(Time.time - lastSkillTime >= 1)
        {
            ResolveRandomMining(MiningType.Laser);
            ResolveRandomMining(MiningType.Bomb);
            lastSkillTime = Time.time;
        }
    }   
    private void ResolveTargetMining(Vector2 position,MiningType type)
    {
        List<StoneActor> stones = DetectOre(position);
        miningSequence.RequestMines(stones, Vector3.zero, type); 
    }

    private void ResolveRandomMining(MiningType type)
    {
        if (!spawner.GetRandomStonePosition(out Vector2Int position2D)) return;
        Vector3 position = GridCalculator.GridToWorld(position2D);
        List<StoneActor> stones = DetectOre(position);
        miningSequence.RequestMines(stones, position, type);
    }
    private List<StoneActor> DetectOre(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, GetMiningRange(), oreLayer);

        Debug.Log($"감지된 광석 수 : {hits.Length}");

        List<StoneActor> stones = new List<StoneActor>();
        foreach (Collider2D hit in hits)
        {
            StoneActor stone = hit.GetComponent<StoneActor>();
            stones.Add(stone);
        }
        return stones;
    }
      

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldPos, GetMiningRange());
    }
}
