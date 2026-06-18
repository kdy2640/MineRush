using TMPro;
using UnityEngine;

public class RuntimeStatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxOreTierText;
    [SerializeField] private TextMeshProUGUI miningPowerText;
    [SerializeField] private TextMeshProUGUI miningSpeedText;
    [SerializeField] private TextMeshProUGUI miningRadiousText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI critMultiplierText;
    [SerializeField] private TextMeshProUGUI extraDurationText;
    [SerializeField] private TextMeshProUGUI rewardMultiplierText;

    private RuntimeStat stat;

    private void Start()
    {

        stat = GameManager.Instance?.Upgrade?.GetRuntimeStat();

    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (stat == null)
        {
            return;
        }

        if (miningPowerText == null)
        {
            return;
        }

        maxOreTierText.text = $"MaxOreTier : {stat.MaxOreTier}";
        miningPowerText.text = $"Power : {stat.MiningPower}";
        miningSpeedText.text = $"Speed : {stat.MiningSpeed}";
        miningRadiousText.text = $"MiningRadious : {stat.MiningRadius}";
        critChanceText.text = $"Crit % : {stat.CriticalChance * 100f}%";
        critMultiplierText.text = $"Crit x : {stat.CriticalMultiplier}";
        extraDurationText.text = $"ExtraDuration : {stat.ExtraDuration}";
        rewardMultiplierText.text = $"RewardMultiplier : {stat.RewardMultiplier}";
    }
}
