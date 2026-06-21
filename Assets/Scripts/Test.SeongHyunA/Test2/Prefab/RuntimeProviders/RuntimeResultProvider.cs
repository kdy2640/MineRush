using System.Collections.Generic;
using UnityEngine;

public class RuntimeResultProvider : MonoBehaviour, IResultProvider
{
    public Dictionary<string, int> GetSessionData()
    {
        return RewardSystem.Instance.GetAll();
    }

    public int GetSessionXP()
    {
        return XPSystem.Instance.GetSessionXP();
    }

    public int GetTotalXP()
    {
        return XPSystem.Instance.GetTotalXP();
    }

    public int GetTotalOre(string oreName)
    {
        return RewardSystem.Instance.GetTotalReward(oreName);
    }
}