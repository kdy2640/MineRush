using System;
using Unity.VisualScripting;
using UnityEngine; 

public class StoneActor : MonoBehaviour
{
    public event Action<StoneActor> OnDead;
    [SerializeField] private StoneDataSO dataSo;

    private Vector2Int gridPos;
    public Vector2Int GridPos => gridPos;
    public StoneDataSO DataSO => dataSo;

    private GameManager manager;
    private void Awake()
    {
        manager = GameManager.Instance;
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
    } 
    public void Mine(float damage)
    {
        Die();
    }

    private void Die()
    {
        manager.OreManager.AddRange(dataSo.RewardList);
        OnDead?.Invoke(this);
    }
}
