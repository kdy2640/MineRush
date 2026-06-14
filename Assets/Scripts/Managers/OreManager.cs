using System;
using System.Collections.Generic;

public class OreManager
{

    private Dictionary<OreType, int> ores = new();
    private Action OnOreChanged;

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

    public bool TrySpend(List<OreAmount> costs)
    {
        if (!HasCost(costs))
            return false;

        foreach (var cost in costs)
        {
            ores[cost.oreType] -= cost.amount;
        }

        OnOreChanged();
        return true;
    }

    public void Add(OreType type, int amount)
    {
        if (!ores.ContainsKey(type))
            ores[type] = 0;

        ores[type] += amount;
        OnOreChanged();
    }

    public void AddRange(List<OreAmount> rewards)
    {
        foreach (var reward in rewards)
        {
            Add(reward.oreType, reward.amount);
        }
    }
    public void SubscribeOreChange(Action ev)
    {
        OnOreChanged += ev;
    }
    public void UnSubscribeOreChange(Action ev)
    {
        OnOreChanged -= ev;
    }
}