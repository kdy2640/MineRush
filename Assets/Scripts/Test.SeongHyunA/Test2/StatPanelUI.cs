using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StatPanelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI maxOreTier;
    [SerializeField] private TextMeshProUGUI miningPower;
    [SerializeField] private TextMeshProUGUI miningSpeed;
    [SerializeField] private TextMeshProUGUI miningRadius;
    [SerializeField] private TextMeshProUGUI critChance;
    [SerializeField] private TextMeshProUGUI critMultiplier;
    [SerializeField] private TextMeshProUGUI extraDuration;
    [SerializeField] private TextMeshProUGUI rewardMultiplier;

    private Coroutine routine;

    private bool isVisible;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void Toggle()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(
            isVisible ? Hide() : Show());
    }

    private IEnumerator Show()
    {
        isVisible = true;

        canvasGroup.blocksRaycasts = true;

        UpdateUI();

        yield return canvasGroup
            .DOFade(1f, 0.2f)
            .WaitForCompletion();
    }

    private IEnumerator Hide()
    {
        isVisible = false;

        canvasGroup.blocksRaycasts = false;

        yield return canvasGroup
            .DOFade(0f, 0.2f)
            .WaitForCompletion();
    }

    private void UpdateUI()
    {
        if (StatSystem.Instance == null)
            return;

        var s = StatSystem.Instance.GetStat();

        maxOreTier.text = $"Tier : {s.MaxOreTier}";
        miningPower.text = $"Power : {s.MiningPower:0.00}";
        miningSpeed.text = $"Speed : {s.MiningSpeed:0.00}";
        miningRadius.text = $"Radius : {s.MiningRadius:0.00}";
        critChance.text = $"Crit : {(s.CriticalChance * 100f):0.0}%";
        critMultiplier.text = $"Crit x{s.CriticalMultiplier:0.00}";
        extraDuration.text = $"Duration : {s.ExtraDuration:0.00}";
        rewardMultiplier.text = $"Reward x{s.RewardMultiplier:0.00}";
    }
}