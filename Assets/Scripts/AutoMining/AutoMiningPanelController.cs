using System;
using System.Collections.Generic; 
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoMiningPanelController : MonoBehaviour
{
    [SerializeField] private AutoMiner autoMiner;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Transform rewardChestTransform;
    private Tween chestTween;
    
    [SerializeField] private float refreshInterval = 1f;
    [SerializeField] private float rewardDisplaySeconds = 10f;
    [SerializeField] private float gaugeInterval = 10f;

    private float refreshTimer;
    private float gaugeTimer;

    private bool isPanelFocus;
    [SerializeField] private RectTransform targetRefreshLayoutRoot;
    [SerializeField] private Slider timerSlider;
    
    private bool needRefreshNextFrame;

    [SerializeField] private GameObject mineVisual;
    [SerializeField] private UI_OrePanel orePanel;
    private void Awake()
    {
        if (autoMiner == null)
            autoMiner = GetComponent<AutoMiner>();
        chestTween = rewardChestTransform.DOShakeScale(0.35f, 0.45f, 10)
            .SetAutoKill(false)
            .Pause()
            .OnComplete(() =>
            {
                rewardChestTransform.localScale = Vector3.one;
            });
    }

    private float CalculateGaugeTimerOffset()
    {
        if (autoMiner.GetLevel() <= 0)
        {
            return 0f;
        }

        if (gaugeInterval <= 0f)
            return 0f;

        float elapsedSeconds = Mathf.Max(
            0f,
            (float)AutoMiningRuntimeData.GetElapsedClaimTime().TotalSeconds
        );

        return elapsedSeconds % gaugeInterval;
    }

    private void OnEnable()
    {
        isPanelFocus = true;
        refreshTimer = 0f;
        gaugeTimer = CalculateGaugeTimerOffset();
        needRefreshNextFrame = true;
        mineVisual.SetActive(isPanelFocus);
        orePanel.SetDelayRefresh(isPanelFocus);
    }

    private void OnDisable()
    {
        isPanelFocus = false;
        mineVisual.SetActive(isPanelFocus);
        orePanel.SetDelayRefresh(isPanelFocus);
        orePanel.RefreshUI();
    }

    private void Update()
    {
        if(!isPanelFocus) return;

        if (needRefreshNextFrame)
        {
            needRefreshNextFrame = false;
            RefreshInfoPanel();
            RefreshSlider();
        }

        if (autoMiner.GetLevel() <= 0)
        {
            RefreshSlider();
            return;
        }

        refreshTimer += Time.deltaTime;
        gaugeTimer += Time.deltaTime;

        if (refreshTimer >= refreshInterval)
        {
            refreshTimer = 0f;
            RefreshInfoPanel();
            PlayAutoMiningSfx();
        }

        if (gaugeTimer >= gaugeInterval)
        {
            gaugeTimer %= gaugeInterval;
            RefreshInfoPanel();
            chestTween.Restart();
        }

        RefreshSlider();
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
            float amountPerDisplaySeconds = amountPerSecond * rewardDisplaySeconds;

            int totalAmount = 0;

            foreach (OreAmount claimReward in claimRewards)
            {
                if (claimReward.oreType != reward.oreType)
                    continue;

                totalAmount = claimReward.amount;
                break;
            }

            string oreIcon = OreTextFormatter.GetTmpTag(reward.oreType);
            string displaySecondsText = rewardDisplaySeconds.ToString("0.##");
            string perDisplaySecondsText = amountPerDisplaySeconds.ToString("0.##");

            rewardTextLines.Add($"{oreIcon}({displaySecondsText}초당 {perDisplaySecondsText}) : {totalAmount}");
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

    private void RefreshInfoPanel()
    {
        RefreshRewardText();
        RefreshCostText();
    }

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

        if (autoMiner.GetLevel() <= 0 || gaugeInterval <= 0f)
        {
            timerSlider.value = 0f;
            return;
        }

        timerSlider.value = Mathf.Clamp01(gaugeTimer / gaugeInterval);
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
        RefreshInfoPanel();
        gaugeTimer = 0f;
        RefreshSlider();
    }

    public void ClaimBtnClick()
    {
        autoMiner.Claim();
        RefreshRewardText();
        gaugeTimer = 0f;
        RefreshSlider();
        GameManager.Instance.AudioManager.PlaySFX(SFXType.OreCollect);
    }
    private void PlayAutoMiningSfx()
    {
        if (autoMiner.GetLevel() <= 0)
            return;

        GameManager.Instance.AudioManager.PlaySFX(SFXType.MetalHit);
    }
}