using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    public static RewardSystem Instance;

    private Dictionary<string, int> sessionRewards =
        new Dictionary<string, int>();

    private Dictionary<string, int> totalRewards =
        new Dictionary<string, int>();

    private void Awake()
    {
        Instance = this;
    }

    public void Add(string oreType, int amount)
    {
        if (!sessionRewards.ContainsKey(oreType))
            sessionRewards.Add(oreType, 0);

        if (!totalRewards.ContainsKey(oreType))
            totalRewards.Add(oreType, 0);

        sessionRewards[oreType] += amount;
        totalRewards[oreType] += amount;
    }

    // ResultUI에서 사용
    public Dictionary<string, int> GetAll()
    {
        return sessionRewards;
    }

    // ResultUI에서 사용
    public int GetTotalReward(string oreType)
    {
        if (!totalRewards.ContainsKey(oreType))
            return 0;

        return totalRewards[oreType];
    }

    // ResultUI에서 사용
    public void Clear()
    {
        sessionRewards.Clear();
    }

    public void ClearSession()
    {
        sessionRewards.Clear();
    }

    public void ResetAll()
    {
        sessionRewards.Clear();
        totalRewards.Clear();
    }
}