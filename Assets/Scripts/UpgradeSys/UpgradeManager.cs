using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // 현재 곡갱이 종류를 추가할지 고민중이라
    // base값을 둘지 말지 안정했기 때문에,
    // 그냥 계산기쪽에서 매번 새 RuntimeStat 객체를 생성하게 해서
    // 중복계산을 막음.
    private List<UpgradeState> upgradeStates; // 나중에 ui가 참고하려면 public으로 바꿔야 하지 않을까?
    private RuntimeStat runtimeStat;
    private StatCalculator statCalculator;
    
    private void Awake()
    {
        runtimeStat = new RuntimeStat(); // 안해도 되지만, 혹시 모를 null값을 위해 new
        statCalculator = new StatCalculator();
        RecalculateRuntimeStat();
    }
    public RuntimeStat GetRuntimeStat()
    {
        return runtimeStat;
    }

    private void RecalculateRuntimeStat()
    {
        runtimeStat = statCalculator.Calculate(upgradeStates);
    }
    
    // TODO 리소스매니저 추가하면 고치기
    // public bool TryBuyUpgrade(UpgradeData data, ResourceManager resource)
    // {
    //     UpgradeState state = GetState(data);
    //     List<OreCost> cost = data.GetCost(state.level);
    //
    //     if (!resource.TrySpend(cost))
    //         return false;
    //
    //     state.level++;
    //     RecalculateRuntimeStat();
    //
    //     return true;
    // }
}
