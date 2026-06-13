using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // 현재 보유한 업그레이드 상태 목록.
    // UI에서 직접 보여줘야 한다면 public으로 열기보다 읽기 전용 프로퍼티를 추가하는 쪽을 우선 고려한다.
    private List<UpgradeState> upgradeStates;
    private RuntimeStat runtimeStat;
    private StatCalculator statCalculator;

    private void Awake()
    {
        runtimeStat = new RuntimeStat(); // 초기 null 방지용. Awake 마지막에 RecalculateRuntimeStat에서 다시 계산된 값으로 교체된다.
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

    // TODO ResourceManager가 추가되면 구매 가능 여부 확인, 비용 차감, 레벨 증가 로직 구현.
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
