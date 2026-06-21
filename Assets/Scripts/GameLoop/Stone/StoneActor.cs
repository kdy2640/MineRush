using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoneActor : MonoBehaviour
{
    public event Action<StoneActor> OnDead;
    [SerializeField] private StoneDataSO dataSo;
    [SerializeField] private float ScaleNoise = 0.1f;

    private Vector2Int gridPos;
    public Vector2Int GridPos => gridPos;
    public StoneDataSO DataSO => dataSo;

    private GameManager manager;
    private HPHandler hpHandler;
    private void Awake()
    {
        manager = GameManager.Instance;
        hpHandler = GetComponent<HPHandler>();
        hpHandler.SubscribeHPUpdate(Mine);
        hpHandler.SubscribeDying(Die);
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
        go.GetComponent<SpriteRenderer>().sortingOrder = 1000 - (GridPos.x + GridPos.y);
        hpHandler.SetMaxHealth(dataSo.MaxHealth);
        RandomAdjust(go);
    } 
    private void RandomAdjust(GameObject solid)
    { 
        if(Random.value < 0.5f)
        {
            solid.GetComponent<SpriteRenderer>().flipX = true;
        }

        solid.transform.localScale = Vector3.one * (1 + (Random.value - 0.5f) * 2 * ScaleNoise); 

    }

    public void Mine(float nowHP)
    {
        Debug.Log($"Stone Mined : Name[{gameObject.name}], nowHP[{nowHP}]"); 
    }

    private void Die()
    { 
        manager.GameLoop.Events.Invoke(GameLoopEventType.StoneDestroyed);
        OnDead?.Invoke(this);
    }


}
