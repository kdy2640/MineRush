using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private float lineSpawnDelay = 0.08f; 
    [SerializeField] private StonePooler pooler;
    
    public static readonly int GRID_MAX_SIZE = 16;
    public static readonly int GRID_RESOLUTION_MULTIPLIER = 2;
    public static readonly float GRID_RESOLUTION_RATIO = 1f / (float)GRID_RESOLUTION_MULTIPLIER;

    public int GRID_SIZE => GRID_MAX_SIZE * GRID_RESOLUTION_MULTIPLIER;


    private readonly Dictionary<Vector2Int, StoneActor> aliveStones = new();

    public int AliveCount => aliveStones.Count;

    private void Start()
    { 
        GameManager.Instance.GameLoop.SkillProxy.SubscribeAction(SkillBase.SkillType.SpawnOreWhenMined, RandomSpawnOne);
    }
    private void OnDestroy()
    { 
        GameManager.Instance.GameLoop.SkillProxy.UnSubscribeAction(SkillBase.SkillType.SpawnOreWhenMined, RandomSpawnOne);
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
        Vector3 worldPos = GridCalculator.GridToWorld(gridPos);

        StoneActor stone = pooler.Get(new StonePoolArgs(data, gridPos));
        stone.transform.position = worldPos;

        stone.gameObject.SetActive(isImmediate);
        if(isImmediate)
        {
            stone.Presenter.PlaySpawnTween();
        }

        aliveStones[gridPos] = stone;
        stone.SubscribeReturnListener(HandleStoneDead); 
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

    private void HandleStoneDead(Poolable stone)
    {
        if(stone is StoneActor)
        {
            StoneActor stoneActor = stone as StoneActor;
            stoneActor.UnSubscribeReturnListener(HandleStoneDead);
            aliveStones.Remove(stoneActor.GridPos);
        }
    }

    public bool GetRandomStonePosition(out Vector2Int position)
    {
        int count = aliveStones.Keys.Count;
        position = Vector2Int.zero;
        if (count == 0) return false;
        int random = Random.Range(0, count);
        position =  aliveStones.Keys.ToList()[random];
        return true;
    }
}