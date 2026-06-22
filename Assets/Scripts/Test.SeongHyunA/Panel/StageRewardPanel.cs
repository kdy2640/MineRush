using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageRewardPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardText;

    private List<OreAmount> rewards = new List<OreAmount>();

    public List<OreAmount> Rewards { get { return rewards; } }

    public void AddReward(OreType oreType, int amount)
    {
        OreAmount target = null;

        foreach (OreAmount reward in rewards)
        {
            if (reward.oreType == oreType)
            {
                target = reward;
                break;
            }
        }

            if (target == null)
            {
                rewards.Add(new OreAmount(oreType, amount));
            }
            else
            {
                target.amount += amount;
            }
        RefreshUI();
        
    }
    public void ClearReward()
    {
        rewards.Clear();
        RefreshUI();
    }
    private void RefreshUI()
    {
        string text = "=== Ore Amount ===\n";

        int totalReward = 0;

        foreach(OreAmount reward in rewards)
        {
            text += $"{reward.oreType} : {reward.amount}\n";

            totalReward += reward.amount;
        }

        text += $"\nTotal : {totalReward}";

        rewardText.text = text;
    }
}
