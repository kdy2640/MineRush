using TMPro;
using UnityEngine;

public class StageRewardPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardText;

    private int currentReward;

    private void Start()
    {
        currentReward = 0;
        RefreshUI();
    }

    public void AddReward(int amount)
    {
        currentReward += amount;
        RefreshUI();
    }

    private void RefreshUI()
    {
        rewardText.text =
            $"Reward : {currentReward}";
    }
}
