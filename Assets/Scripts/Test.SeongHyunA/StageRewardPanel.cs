using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageRewardPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardText;

    private OreManager oreManager;

    private void Start()
    {
        oreManager = GameManager.Instance.OreManager;

        oreManager.SubscribeOreChange(RefreshUI);

        RefreshUI();
    }


    private void OnDestroy()
    {
        if (oreManager != null)
        {
            oreManager.UnSubscribeOreChange(RefreshUI);
        }
    }

    private void RefreshUI()
    {
        string text = "====Reward Amount====\n";

        int totalReward = 0;

        foreach (OreType oreType in System.Enum.GetValues(typeof(OreType)))
        {
            if (oreType == OreType.None || oreType == OreType.Length)
                continue;

            int amount = oreManager.GetAmount(oreType);

            text += $"{oreType} : {amount}\n";

            totalReward += amount;
        }

        text += $"\nTotal : {totalReward}";

        rewardText.text = text ;
    }
}
