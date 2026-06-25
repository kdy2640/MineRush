using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LightTransport;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private float lineSpawnDelay = 0.08f;
    [SerializeField] private StoneActor stoneActorPrefab;

    public readonly int GRID_MAX_SIZE = 16;
    public readonly int GRID_RESOLUTION_MULTIPLIER = 2;

    public int GRID_SIZE => GRID_MAX_SIZE * GRID_RESOLUTION_MULTIPLIER;


    private readonly Dictionary<Vector2Int, StoneActor> aliveStones = new();

    public int AliveCount => aliveStones.Count;

    private void Start()
    { 
        GameManager.Instance.GameLoop.SkillProxy.SubscribeAction(SkillBase.SkillType.SpawnOreWhenMined, RandomSpawnOne);
    }
     
    public IEnumerator PrepareRoutine()
    {
        RandomSpawn(GameManager.Instance.Upgrade.GetRuntimeStat().StoneCount, false);
        yield return null;
    }

    public IEnumerator PreStartRoutine()
    {
        for (int y = GRID_SIZE - 1; y >= 0; y--)
        {
            bool hasStoneInLine = false;

            for (int x = 0; x < GRID_SIZE; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);

                if (!aliveStones.TryGetValue(gridPos, out StoneActor stone))
                    continue;

                stone.gameObject.SetActive(true);

                if (stone.TryGetComponent(out StonePresenter presenter))
                    StartCoroutine(presenter.PlaySpawnRoutine());

                hasStoneInLine = true;
            }

            if (hasStoneInLine)
                yield return new WaitForSeconds(lineSpawnDelay);
        }
    }

    public void RandomSpawnOne()
    {
        RandomSpawn(1,true);
    }
     
    public void RandomSpawn(int spawnCount, bool isImmediate)
    { 
        int maxStoneCount = GRID_SIZE * GRID_SIZE;
        int availableCount = maxStoneCount - aliveStones.Count;

        if (availableCount <= 0) return;

        spawnCount = Mathf.Min(spawnCount, availableCount);

        int spawnedCount = 0;

        while (spawnedCount < spawnCount)
        {
            int x = Random.Range(0, GRID_SIZE);
            int y = Random.Range(0, GRID_SIZE);

            Vector2Int gridPos = new Vector2Int(x, y);

            if (aliveStones.ContainsKey(gridPos)) continue;

            StoneDataSO randomData = GetStoneDataSO();

            SpawnStone(randomData, gridPos, isImmediate);
            spawnedCount++;
        }
    }

    private void SpawnStone(StoneDataSO data, Vector2Int gridPos, bool isImmediate)
    {
        Vector3 worldPos = GridCalculator.GridToWorld(gridPos, (float)1 / (float)GRID_RESOLUTION_MULTIPLIER);

        StoneActor stone = Instantiate(stoneActorPrefab, worldPos, Quaternion.identity);
        stone.SetData(data, gridPos);
        stone.gameObject.SetActive(isImmediate);

        aliveStones[gridPos] = stone;
        stone.OnDead += HandleStoneDead;
    }

    private StoneDataSO GetStoneDataSO()
    {
        RuntimeStat stat = GameManager.Instance.Upgrade.GetRuntimeStat();
        int nowMaxOreTierIndex = stat.MaxOreTier;

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
    }
}