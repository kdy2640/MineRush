using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // 현재 보유한 업그레이드 상태 목록.
    // UI에서 직접 보여줘야 한다면 public으로 열기보다 읽기 전용 프로퍼티를 추가하는 쪽을 우선 고려한다.
    [SerializeField] private List<UpgradeState> upgradeStates = new();
    [SerializeField] private RuntimeStat runtimeStat;
    // 조회용 UpgradeStateDictionary
    private Dictionary<string, UpgradeState> upgradeStateMap = new();
    private StatCalculator statCalculator;
    private OreManager oreManager;
    private void Awake()
    {
        runtimeStat = new RuntimeStat(); // 초기 null 방지용. Awake 마지막에 RecalculateRuntimeStat에서 다시 계산된 값으로 교체된다.
        statCalculator = new StatCalculator();

        BuildUpgradeStateMap();
        RecalculateRuntimeStat();
    }

    private void Start()
    {
        oreManager = GameManager.Instance.OreManager;   
    }
    public RuntimeStat GetRuntimeStat()
    {
        return runtimeStat;
    }


    public UpgradeState GetState(UpgradeData data)
    {
        if (data == null)
        {
            Debug.LogError("UpgradeData가 null입니다.");
            return null;
        }

        if (string.IsNullOrEmpty(data.id))
        {
            Debug.LogError($"{data.name}의 id가 비어 있습니다.");
            return null;
        }

        if (upgradeStateMap.TryGetValue(data.id, out UpgradeState state))
            return state;

        state = new UpgradeState
        { 
            data = data,
            level = 0
        };

        upgradeStates.Add(state);
        upgradeStateMap.Add(data.id, state);

        return state;
    }
    // 당장 읽기전용은 안 되지만 만든 조회 메소드
    private void BuildUpgradeStateMap()
    {
        upgradeStateMap.Clear();

        foreach (UpgradeState state in upgradeStates)
        {
            if (state == null) continue; 
            if (state.data == null) continue; 
            if (string.IsNullOrEmpty(state.data.id))  continue; 
            if (upgradeStateMap.ContainsKey(state.data.id))
            {
                Debug.LogWarning($"중복된 UpgradeData id가 있습니다: {state.data.id}");
                continue;
            } 
            upgradeStateMap.Add(state.data.id, state);
        }
    }


    [ContextMenu("Recalculate Runtime Stat")]
    private void RecalculateRuntimeStatForTest()
    {
        RecalculateRuntimeStat();
    }

    private void RecalculateRuntimeStat()
    {
        if (statCalculator == null)
        {
            statCalculator = new StatCalculator();
        }

        runtimeStat = statCalculator.Calculate(upgradeStates);
    }
     
    public bool TryBuyUpgrade(UpgradeData data)
    {
        UpgradeState state = GetState(data);
        List<OreAmount> cost = data.GetCosts(state.level);
    
        if (!oreManager.TrySpend(cost))
            return false;
    
        state.level++;
        RecalculateRuntimeStat();
    
        return true;
    }
}
