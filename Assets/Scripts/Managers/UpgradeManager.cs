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
        if (IsMaxLevel(state))
        {
            return false;
        }
        List<OreAmount> cost = data.GetCosts(state.level);
        
        if (!oreManager.TrySpend(cost))
        {
            return false;
        }
    
        state.level++;
        RecalculateRuntimeStat();
        if (state.data.skill != null)
        {
            GameManager.Instance.SkillManager.SetSkillLevel(state.data.skill.id, state.level);
        }

        return true;
    }

    public bool IsMaxLevel(UpgradeState state)
    {
        return state.level >= state.data.maxLevel;
    }

    /// <summary>
    /// 노드에서 새로운 노드가 열렸을 때 그게 스킬 업그레이드면 스킬매니저에 등록 및 스폰하는 함수
    /// 만약 그런방식이 아니라 처음부터 노드가 싹 다 열려있는거면 그냥 모든 스킬 스킬매니저에 0레벨로 등록하면 됨.
    /// </summary>
    public void RegisterSkillWhenUnlockNode(UpgradeState state)
    {
        GameManager.Instance.SkillManager.RegisterSkill(state.data.skill);
        GameManager.Instance.SkillManager.SetSkillLevel(state.data.skill.id, 0);
    }
    
    public bool HasState(UpgradeData data)
    {
        if (data == null)
        {
            Debug.Log("전달받은 데이터so가 비어있음.");
            return false;
        }

        if (string.IsNullOrEmpty(data.id))
        {
            Debug.Log("전달받은 데이터so의 id가 비어있음");
            return false;
        }
        return upgradeStateMap.ContainsKey(data.id);
    }
}
