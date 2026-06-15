using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public readonly int GRID_MAX_SIZE = 16;

    [SerializeField] private StoneActor stoneActorPrefab;
    [SerializeField] private List<StoneDataSO> stoneDataList = new();

    private readonly Dictionary<Vector2Int, StoneActor> aliveStones = new();

    public int AliveCount => aliveStones.Count;

    public void RandomSpawn(int spawnCount)
    {
        if (stoneDataList == null || stoneDataList.Count == 0)
        {
            Debug.Log("StoneDataSO가 등록되어 있지 않습니다.");
            return;
        }

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

            StoneDataSO randomData = stoneDataList[Random.Range(0, stoneDataList.Count)];

            SpawnStone(randomData, gridPos);
            spawnedCount++;
        }
    }

    public void SpawnStone(StoneDataSO data, Vector2Int gridPos)
    {
        Vector3 worldPos = GridCalculator.GridToWorld(gridPos);

        StoneActor stone = Instantiate(stoneActorPrefab, worldPos, Quaternion.identity);
        stone.SetData(data, gridPos);

        aliveStones[gridPos] = stone;
        stone.OnDead += HandleStoneDead;
    }

    private void HandleStoneDead(StoneActor stone)
    {
        stone.OnDead -= HandleStoneDead;
        aliveStones.Remove(stone.GridPos);

        Destroy(stone.gameObject);
    }
}