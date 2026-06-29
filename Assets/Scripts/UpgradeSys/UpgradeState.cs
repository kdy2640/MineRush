using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어가 어떤 업그레이드를 보유하고 있는지 나타내기 위한 데이터.<br/>
/// 실질적으로 이게 업그레이드 노드 데이터 그 자체라고 볼 수 있다.<br/><br/>
/// level이 UpgradeData안에 있지 않고 여기에 따로 빼서 포장하는 이유는 <br/>
/// 레벨은 인게임에서 실시간으로 변하는데, SO의 값을 실시간으로 고치는건 좋은 방식이 아니다.
/// </summary>
[System.Serializable]
public class UpgradeState
{
    public UpgradeData data; // ScriptableObject 참조
    public int level;

    public List<OreAmount> GetCurrentCost()
    {
        if (data.levelBasedCosts != null && data.levelBasedCosts.Count > 0)
            return GetLevelBasedCost();
        return data.GetCosts(level);
    }
    private List<OreAmount> GetLevelBasedCost()
    {
        List<OreAmount> costs = new();

        foreach (LevelBasedOreCost cost in data.levelBasedCosts)
        {
            if (level < cost.startLevel)
                continue;

            int effectiveLevel = level - cost.startLevel;
            float calculatedAmount = cost.baseAmount * Mathf.Pow(cost.amountMultiplier, effectiveLevel);
            int finalAmount = Mathf.RoundToInt(calculatedAmount);

            costs.Add(new OreAmount(cost.oreType, finalAmount));
        }

        return costs;
    }
}
