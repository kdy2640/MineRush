using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;

public class StoneSpawner : MonoBehaviour
{
    public readonly int GRID_MAX_SIZE = 16;

    [SerializeField] private StoneActor stoneActorPrefab; 

    private readonly Dictionary<Vector2Int, StoneActor> aliveStones = new();

    public int AliveCount => aliveStones.Count;

    private void Start()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopStarted,OnLoopStarted);
    }

    private void OnDestroy()
    { 
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopStarted, OnLoopStarted);
    }

    public void OnLoopStarted()
    {
        transform.position = Vector3.zero;
        RandomSpawn(GameManager.Instance.Upgrade.GetRuntimeStat().StoneCount);
    }
     
    public void RandomSpawn(int spawnCount)
    { 
        int maxStoneCount = GRID_MAX_SIZE * GRID_MAX_SIZE;
        int availableCount = maxStoneCount - aliveStones.Count;

        if (availableCount <= 0) return;

        spawnCount = Mathf.Min(spawnCount, availableCount);

        int spawnedCount = 0;

        while (spawnedCount < spawnCount)
        {
            int x = Random.Range(0, GRID_MAX_SIZE);
            int y = Random.Range(0, GRID_MAX_SIZE);

            Vector2Int gridPos = new Vector2Int(x, y);

            if (aliveStones.ContainsKey(gridPos)) continue;

            StoneDataSO randomData = GetStoneDataSO();

            SpawnStone(randomData, gridPos);
            spawnedCount++;
        }
    }

    private void SpawnStone(StoneDataSO data, Vector2Int gridPos)
    {
        Vector3 worldPos = GridCalculator.GridToWorld(gridPos);

        StoneActor stone = Instantiate(stoneActorPrefab, worldPos, Quaternion.identity);
        stone.SetData(data, gridPos);

        aliveStones[gridPos] = stone;
        stone.OnDead += HandleStoneDead;
    }

    private StoneDataSO GetStoneDataSO()
    {
        RuntimeStat stat = GameManager.Instance.Upgrade.GetRuntimeStat();
        int nowMaxOreTierIndex = stat.MaxOreTier - 1;

        for (int i = nowMaxOreTierIndex; i >= 0; i--)
        {
            OreType nowType = (OreType)i;
            if (Random.value < stat.GetOrePureChance(nowType))
            {
                return StoneDataDB.GetStoneDataSO(StoneDataDB.StoneType.Pure, nowType);
            }
            if (Random.value < stat.GetOreFragmentChance(nowType))
            {
                return StoneDataDB.GetStoneDataSO(StoneDataDB.StoneType.Fragment, nowType);
            }
        }
        return StoneDataDB.GetStoneDataSO(StoneDataDB.StoneType.Base, OreType.Copper);

    } 

    private void HandleStoneDead(StoneActor stone)
    {
        stone.OnDead -= HandleStoneDead;
        aliveStones.Remove(stone.GridPos);

        Destroy(stone.gameObject);
    }
}