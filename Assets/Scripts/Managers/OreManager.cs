using System;
using System.Collections.Generic;
using UnityEngine;


// 플레이어의 현재 Ore 상태를 관리하는 매니저
// Ore상태 변동시 콜백이 필요하면 Subscribe/UnSubscribe 함수를 사용해주세요.
public class OreManager : MonoBehaviour
{

    private Dictionary<OreType, int> ores = new();
    private Action OnOreChanged;


#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private List<OreAmount> debugOres = new();
#endif


    public int GetAmount(OreType type)
    {
        return ores.TryGetValue(type, out int amount) ? amount : 0;
    }

    public bool HasCost(List<OreAmount> costs)
    {
        foreach (var cost in costs)
        {
            if (GetAmount(cost.oreType) < cost.amount)
                return false;
        }

        return true;
    }
    
    // 손유민 : UI에서 광물당 개별로 색상 표시하기 위해 개별 체크함수 추가했습니다.
    // 오버로딩하기엔 함수 네이밍이 좀 방향성에 안맞기도 하고 고치는것보단 추가하는게 나을 것 같아서요.
    public bool HasEnoughOre(OreAmount oreAmount)
    {
        return GetAmount(oreAmount.oreType) >= oreAmount.amount;
    }

    public bool TrySpend(List<OreAmount> costs)
    {
        if (!HasCost(costs))
            return false;

        foreach (var cost in costs)
        {
            ores[cost.oreType] -= cost.amount;
        }

        OnOreChanged?.Invoke();
        return true;
    }

    private void Add(OreType type, int amount)
    {
        if (!ores.ContainsKey(type))
            ores[type] = 0;

        ores[type] += amount;
    }

    public void AddRange(IReadOnlyList<OreAmount> rewards)
    {
        foreach (var reward in rewards)
        {
            Add(reward.oreType, reward.amount);
        }
        OnOreChanged?.Invoke();
    }
    public void SubscribeOreChange(Action ev)
    {
        OnOreChanged += ev;
    }
    public void UnSubscribeOreChange(Action ev)
    {
        OnOreChanged -= ev;
    }

    // 디버깅용 -> 업그레이드에서 광석수 증가 감소 및 시각화
#if UNITY_EDITOR
    [ContextMenu("Debug/Apply Debug Ores")]
    private void ApplyDebugOres()
    {
        ores.Clear();

        foreach (var ore in debugOres)
        {
            ores[ore.oreType] = ore.amount;
        }

        OnOreChanged?.Invoke();
    }

    [ContextMenu("Debug/Sync Current Ores To Debug List")]
    private void SyncCurrentOresToDebugList()
    {
        debugOres.Clear();

        foreach (var pair in ores)
        {
            debugOres.Add(new OreAmount(pair.Key, pair.Value));
        }
    }

    [ContextMenu("Debug/Clear Ores")]
    private void ClearOres()
    {
        ores.Clear();
        OnOreChanged?.Invoke();
    }
#endif

}