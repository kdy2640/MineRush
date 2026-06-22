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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Add(
        OreType oreType,
        int amount)
    {
        string key = oreType.ToString();

        if (!sessionRewards.ContainsKey(key))
            sessionRewards.Add(key, 0);

        if (!totalRewards.ContainsKey(key))
            totalRewards.Add(key, 0);

        sessionRewards[key] += amount;
        totalRewards[key] += amount;
    }

    public Dictionary<string, int> GetAll()
    {
        return sessionRewards;
    }

    public int GetTotalReward(
        string oreType)
    {
        if (!totalRewards.ContainsKey(oreType))
            return 0;

        return totalRewards[oreType];
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
