using TMPro;
using UnityEngine;

public class RuntimeStatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI miningPowerText;
    [SerializeField] private TextMeshProUGUI miningSpeedText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI critMultiplierText;

    private RuntimeStat stat;

    private void Start()
    {
        // GameManager 통해 RuntimeStat 가져온다고 가정
        stat = GameManager.Instance.Upgrade.GetRuntimeStat();

        RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        miningPowerText.text = $"Power : {stat.MiningPower}";
        miningSpeedText.text = $"Speed : {stat.MiningSpeed}";
        critChanceText.text = $"Crit % : {stat.CriticalChance * 100f}%";
        critMultiplierText.text = $"Crit x : {stat.CriticalMultiplier}";
    }
}
