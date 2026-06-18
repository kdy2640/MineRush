using TMPro;
using UnityEngine;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardText;

    public void SetText(string text)
    {
        rewardText.text = text;
    }
}
