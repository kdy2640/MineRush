using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StoneSortingOrder
{
    public const int Step = 10;

}

public class StonePoolArgs : PoolArgs
{
    public StoneDataSO dataSO;
    public Vector2Int gridPos;

    public StonePoolArgs(StoneDataSO dataSO, Vector2Int gridPos)
    {
        this.dataSO = dataSO;
        this.gridPos = gridPos;
    }
}
    public class StoneActor : Poolable
{ 
    [SerializeField] private StoneDataSO dataSo;
    [SerializeField] private SpriteRenderer solidRenderer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private float ScaleNoise = 0.1f;
    [SerializeField] private float CrackHealthRatio = 0.5f;

    private bool nowCracked = false;
    private GameManager manager;
    private HPHandler hpHandler;
    private StonePresenter presenter;

    private Vector2Int gridPos;
    public Vector2Int GridPos => gridPos;
    public StoneDataSO DataSO => dataSo;
    public HPHandler HP => hpHandler;
    public StonePresenter Presenter => presenter;

    private void Awake()
    {
        manager = GameManager.Instance;
        presenter = GetComponent<StonePresenter>();
        hpHandler = GetComponent<HPHandler>();  
        hpHandler.SubscribeHPUpdate(Mine);
        presenter.SubscribeBreakRoutineEnd(Die);
        presenter.SubscribeSpawnRoutineEnd(OnSpawnRoutineEnd);

        presenter.SubscribeBreakRoutineDelay(DelayDie);
    }
    private void OnDestroy()
    {
        hpHandler.UnSubscribeHPUpdate(Mine);
        presenter.UnSubscribeBreakRoutineEnd(Die);
        presenter.UnSubscribeSpawnRoutineEnd(OnSpawnRoutineEnd);
        presenter.UnSubscribeBreakRoutineDelay(DelayDie);
    }

    public bool TryStartDeathSequence()
    {
        if (!HP.IsDead)
            return false;

        if (presenter.DeathSequenceStarted)
            return false;

        presenter.DeathSequenceStarted = true;
        return true;
    }

    public void SetData(StoneDataSO data, Vector2Int gridPos)
    {
        this.dataSo = data;
        this.gridPos = gridPos;
        Initialize();
    }
    public void Initialize()
    { 
        solidRenderer.sprite = dataSo.StoneSprite;  
        int sortingOrder = 10000 - StoneSortingOrder.Step * (GridPos.x + GridPos.y);
        solidRenderer.sortingOrder = sortingOrder; 

        RandomAdjust(solidRenderer.transform,ScaleNoise); 

        hpHandler.SetMaxHealth(dataSo.MaxHealth);
        nowCracked = false;
    }

    public void RandomAdjust(Transform transform, float ScaleNoise)
    {
        transform.localScale = Vector3.one * (1 + (Random.value - 0.5f) * 2 * ScaleNoise);
        SetFlip(transform, Random.value < 0.5f);

    }
    private void SetFlip(Transform transform,bool isFlip)
    {
        float offset = isFlip ? 1 : -1;
        transform.localScale = new Vector3(offset * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void Mine(float nowHP)
    {
        nowCracked = nowHP / hpHandler.maxHp < CrackHealthRatio; 
    }

    public void CheckCrack()
    {
        if(nowCracked)
        {
            solidRenderer.sprite = dataSo.StoneCrackSprite;
        } 
    }

    private void Die()
    {
        collider.enabled = false;
        manager.GameLoop.Events.Invoke(GameLoopEventType.StoneDestroyed);
    }
    private void DelayDie()
    { 
        RequestReturn();
    }
    public override void Initialize(PoolArgs obj)
    {
        if(obj is StonePoolArgs)
        {
            StonePoolArgs args = obj as StonePoolArgs;
            SetData(args.dataSO, args.gridPos);
            presenter.Initialize();
        } 
    }
    private void OnSpawnRoutineEnd()
    { 
        collider.enabled = true;
    }

    public override void ResetState()
    { 

    }
     
}
