using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoneActor : MonoBehaviour
{
    public event Action<StoneActor> OnDead; 
    [SerializeField] private StoneDataSO dataSo;
    [SerializeField] private float ScaleNoise = 0.1f;
    [SerializeField] private float CrackHealthRatio = 0.5f;

    private bool nowCracked = false;
    private GameManager manager;
    private HPHandler hpHandler;
    private StoneViewSorter sorter;

    private Vector2Int gridPos;
    public Vector2Int GridPos => gridPos;
    public StoneDataSO DataSO => dataSo;

    private void Awake()
    {
        manager = GameManager.Instance;
        hpHandler = GetComponent<HPHandler>(); 
        hpHandler.SubscribeDying(Die);
        hpHandler.SubscribeHPUpdate(Mine);
    }
    private void OnDestroy()
    {
        hpHandler.UnSubscribeHPUpdate(Mine);
        hpHandler.UnSubscribeDying(Die); 
    }
    public void SetData(StoneDataSO data, Vector2Int gridPos)
    {
        this.dataSo = data;
        this.gridPos = gridPos;
        Initialize();
    }
    public void Initialize()
    {
        GameObject go = GameObject.Instantiate(dataSo.SolidStonePrefab,transform);
        go.transform.localPosition = Vector3.zero;
        sorter = go.GetComponent<StoneViewSorter>();
        int sortingOrder = 10000 - StoneSortingOrder.Step * (GridPos.x + GridPos.y);
        sorter.SetSorting(sortingOrder);
        sorter.RandomAdjust(ScaleNoise); 
        hpHandler.SetMaxHealth(dataSo.MaxHealth);
        nowCracked = false;
    }  

    public void Mine(float nowHP)
    {
        nowCracked = nowHP / hpHandler.maxHp < CrackHealthRatio; 
    }

    public void CheckCrack()
    {
        sorter.SetCrack(nowCracked);
    }

    private void Die()
    { 
        manager.GameLoop.Events.Invoke(GameLoopEventType.StoneDestroyed);
        OnDead?.Invoke(this);
    }


}
