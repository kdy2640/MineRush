using UnityEngine;

public class RuntimeStatProvider :
    MonoBehaviour,
    IStatProvider
{
    public string GetMaxOreTier()
    {
        return StatSystem.Instance
            .GetStat()
            .MaxOreTier
            .ToString();
    }

    public string GetMiningPower()
    {
        return StatSystem.Instance
            .GetStat()
            .MiningPower
            .ToString("0.00");
    }

    public string GetMiningSpeed()
    {
        return StatSystem.Instance
            .GetStat()
            .MiningSpeed
            .ToString("0.00");
    }

    public string GetMiningRadius()
    {
        return StatSystem.Instance
            .GetStat()
            .MiningRadius
            .ToString("0.00");
    }

    public string GetCritChance()
    {
        return
            (StatSystem.Instance
                .GetStat()
                .CriticalChance * 100f)
            .ToString("0.0")
            + "%";
    }

    public string GetCritMultiplier()
    {
        return "x" +
            StatSystem.Instance
                .GetStat()
                .CriticalMultiplier
                .ToString("0.00");
    }

    public string GetExtraDuration()
    {
        return StatSystem.Instance
            .GetStat()
            .ExtraDuration
            .ToString("0.00");
    }

    public string GetRewardMultiplier()
    {
        return "x" +
            StatSystem.Instance
                .GetStat()
                .RewardMultiplier
                .ToString("0.00");
    }
}