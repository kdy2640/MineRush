using UnityEngine;

public class RuntimeStatPanel : MonoBehaviour
{
    [SerializeField] private RuntimeStatItem itemPrefab;

    [SerializeField] private Transform statContainer;

    private bool isInitialized;

    private void Awake()
    {
        isInitialized = itemPrefab != null && statContainer != null;
        
        if(!isInitialized)
        {
            Debug.LogError($"[{nameof(RuntimeStatPanel)}]초기화 실패.");
        }
    }
    public void SetData(RuntimeStat stat)
    {
        if(stat==null)
        {
            Debug.LogWarning("[RuntimeStatPanel] 전달된 RuntimeStat 데이터가 null입니다.");
            return;
        }
        if(itemPrefab == null || statContainer == null)
        {
            Debug.LogError("[RuntimeStatPanel] itemPrefab 또는 statContainer가 인스펙터에 연결되지 않았습니다.");
            return;
        }

        Clear();

        CreateItem("PICKAXE TIER", stat.PickaxeTier.ToString());

        CreateItem("MINING POWER", stat.MiningPower.ToString("F1"));

        CreateItem("MINING SPEED", stat.MiningSpeed.ToString("F1"));

        CreateItem("MINING RADIUS", stat.MiningRadius.ToString("F1"));

        CreateItem("CRITICAL CHANCE", $"{stat.CriticalChance * 100f:F0}%");

        CreateItem("CRITICAL MULTI", $"{stat.CriticalMultiplier:F1}x");

        CreateItem("EXTRA TIME", $"{stat.ExtraDuration:F1}s");

        CreateItem("REWARD MULTI", $"{stat.RewardMultiplier:F1}x");

        CreateItem("MAX ORE TIER", stat.MaxOreTier.ToString());

        CreateItem("STONE COUNT", stat.StoneCount.ToString());
    }
    private void CreateItem(string statName, string value)
    {
        if(itemPrefab == null) return;

        RuntimeStatItem statItem = Instantiate(itemPrefab, statContainer);

        if(statItem != null)
        {
            statItem.SetData(statName, value);
        }
    }
    private void Clear()
    {
        if (statContainer == null) return;

        for (int i = statContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = statContainer.GetChild(i);
            if (child != null)
            {
                if (child == null) continue;
                Destroy(child.gameObject);
            }
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
