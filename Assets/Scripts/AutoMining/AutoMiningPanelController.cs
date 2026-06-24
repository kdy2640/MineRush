using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoMiningPanelController : MonoBehaviour
{
    [SerializeField] private AutoMiner autoMiner;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text costText;
    
    [SerializeField] private float refreshInterval = 1f;
    private float refreshTimer;

    private bool isPanelFocus;
    [SerializeField] private RectTransform targetRefreshLayoutRoot;
    [SerializeField] private Slider timerSlider;
    
    private bool needRefreshNextFrame;
    private void Awake()
    {
        if (autoMiner == null)
            autoMiner = GetComponent<AutoMiner>();
    }
    private float CalculateRefreshTimerOffset()
    {
        if (refreshInterval <= 0f)
            return 0f;

        float elapsedSeconds = Mathf.Max(
            0f,
            (float)AutoMiningRuntimeData.GetElapsedClaimTime().TotalSeconds
        );

        return elapsedSeconds % refreshInterval;
    }

    private void OnEnable()
    {
        isPanelFocus = true;
        refreshTimer = CalculateRefreshTimerOffset();
        needRefreshNextFrame = true;
    }

    private void OnDisable()
    {
        isPanelFocus = false;
    }

    private void Update()
    {
        if(!isPanelFocus) return;
        if (needRefreshNextFrame)
        {
            needRefreshNextFrame = false;
            RefreshRewardText();
            RefreshCostText();
            RefreshSlider();
        }

        if (autoMiner.GetLevel() > 0)
        {
            refreshTimer += Time.deltaTime;
            if (refreshTimer >= refreshInterval)
            {
                refreshTimer = 0f;

                RefreshRewardText();
            }
            RefreshSlider();
        }
    }

    private string GetAllOreRewardText()
    {
        int currentLevel = autoMiner.GetLevel();
        List<LevelBasedOreReward> rewardData = autoMiner.Data.levelBasedRewards;
        List<OreAmount> claimRewards = autoMiner.CalculateClaimRewards();

        if (currentLevel <= 0 || rewardData == null || rewardData.Count <= 0)
        {
            return "보상 없음";
        } // 자동채굴이 해금되지 않았거나 보상 데이터가 없으면 표시할 보상이 없다.

        List<string> rewardTextLines = new();

        foreach (LevelBasedOreReward reward in rewardData)
        {
            if (currentLevel < reward.startLevel)
                continue;

            int effectiveLevel = currentLevel - reward.startLevel;
            float amountPerSecond = reward.amountPerSecond + reward.amountPerLevel * effectiveLevel;

            int totalAmount = 0;

            foreach (OreAmount claimReward in claimRewards)
            {
                if (claimReward.oreType != reward.oreType)
                    continue;

                totalAmount = claimReward.amount;
                break;
            }

            string oreIcon = OreTextFormatter.GetTmpTag(reward.oreType);
            string perSecondText = amountPerSecond.ToString("0.##");

            rewardTextLines.Add($"{oreIcon}(1초당 {perSecondText}) : {totalAmount}");
        }

        if (rewardTextLines.Count <= 0)
        {
            return "보상 없음";
        } // 현재 레벨에서 startLevel 조건을 만족하는 보상이 없으면 표시할 보상이 없다.

        return string.Join("\n", rewardTextLines);
    }
    private void SetRewardText(string value)
    {
        if (rewardText.text == value)
            return;

        rewardText.text = value;
    } // tmp 자주 호출 될때 성능 하락 방지용.
    //어차피 1초마다 갱신이긴 하지만 그래도 만들어놓음.
    private void RefreshRewardText()
    {
        SetRewardText(GetAllOreRewardText());
        RebuildLayout();
    }

    private void RefreshCostText()
    {
        costText.text = UpgradeOreCostTextFormatter.GetAllOreCostText(
            autoMiner.GetCurrentCost(),
            GameManager.Instance.OreManager
        );
        RebuildLayout();
    }

    private void RefreshSlider()
    {
        if(timerSlider == null) return;
        timerSlider.value = Mathf.Clamp01(refreshTimer / refreshInterval);
    }
    private void RebuildLayout()
    {
        if (targetRefreshLayoutRoot == null)
            return;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetRefreshLayoutRoot);
    }

    public void UpgradeBtnClick()
    {
        if(!autoMiner.TryUpgradeWithClaim()) return;
        RefreshRewardText();
        RefreshCostText();
        refreshTimer = 0f;
        RefreshSlider();
    }

    public void ClaimBtnClick()
    {
        autoMiner.Claim();
        RefreshRewardText();
        refreshTimer = 0f;
        RefreshSlider();
    }
}
