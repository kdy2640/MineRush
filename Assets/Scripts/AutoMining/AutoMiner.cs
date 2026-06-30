using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    [SerializeField] private UpgradeData autoMiningUpgradeData;
    // �ν����Ϳ��� �ڵ�ä�� ���� UpgradeData SO�� �ִ´�.
    [SerializeField] private OreGainPresenter presenter;
    [SerializeField] private GameObject minevisual;

    private const float RewardIntervalSeconds = 10f;

    public UpgradeData Data => autoMiningUpgradeData;
    private UpgradeState state;
    public UpgradeState State
    {
        get
        {
            if (state == null)
                state = GameManager.Instance.Upgrade.GetState(autoMiningUpgradeData);

            return state;
        }
    }
    // ĳ���صα�. �Ƹ� �۵� �ɵ�.
    private void Awake()
    {
        
    }

    private void Start()
    {
        state = GameManager.Instance.Upgrade.GetState(autoMiningUpgradeData);
    } // �ڵ�ä�� �ð� �����Ͱ� �ʱ�ȭ���� �ʾҴٸ� ���� �ð����� �ʱ�ȭ�Ѵ�.

    public int GetLevel()
    {
        return State.level;
    } // ���� �ڵ�ä�� ���׷��̵� ������ ��ȯ�Ѵ�.
    // ui���� ���緹�� ǥ�翡 ���� ����?

    public List<OreAmount> GetCurrentCost()
    {
        int currentLevel = GetLevel();
        List<OreAmount> result = new();

        foreach (LevelBasedOreCost cost in autoMiningUpgradeData.levelBasedCosts)
        {
            if (currentLevel < cost.startLevel)
                continue;

            int effectiveLevel = currentLevel - cost.startLevel;
            int amount = Mathf.RoundToInt(cost.baseAmount * Mathf.Pow(cost.amountMultiplier, effectiveLevel));

            if (amount <= 0)
                continue;

            result.Add(new OreAmount(cost.oreType, amount));
        }

        return result;
    } // ���� �������� ���� ������ �ø� �� �ʿ��� �ڵ�ä�� ����� ����Ѵ�.

    public List<OreAmount> CalculateClaimRewards()
    {
        int currentLevel = State.level;
        List<OreAmount> result = new();

        if (currentLevel <= 0)
            return result;

        if (autoMiningUpgradeData.levelBasedRewards == null)
            return result;

        float elapsedSeconds = Mathf.Max(0f, (float)AutoMiningRuntimeData.GetElapsedClaimTime().TotalSeconds);
        int rewardTickCount = Mathf.FloorToInt(elapsedSeconds / RewardIntervalSeconds);

        if (rewardTickCount <= 0)
            return result;

        foreach (LevelBasedOreReward reward in autoMiningUpgradeData.levelBasedRewards)
        {
            if (currentLevel < reward.startLevel)
                continue;

            int effectiveLevel = currentLevel - reward.startLevel;
            float rewardPerSecond = reward.amountPerSecond + reward.amountPerLevel * effectiveLevel;
            float rewardPerInterval = rewardPerSecond * RewardIntervalSeconds;
            int amount = Mathf.RoundToInt(rewardPerInterval * rewardTickCount);

            if (amount <= 0)
                continue;

            result.Add(new OreAmount(reward.oreType, amount));
        }

        return result;
    } // ���� ���� ���� 10�ʴ� ���� �Ϸ�� 10�� ���� Ƚ���� ���� ���� ���� ������ ����Ѵ�.

    public bool TryUpgradeWithClaim()
    {
        if (GameManager.Instance.Upgrade.IsMaxLevel(state))
            return false;

        List<OreAmount> costs = GetCurrentCost();

        if (!GameManager.Instance.OreManager.TrySpend(costs))
            return false;

        Claim();
        state.level++;
        AutoMiningRuntimeData.ResetClaimTime();
        GameManager.Instance.Save.SaveGame();

        return true;
    } // ���� ���� ������ ���� ������ ��, ����� �����ϰ� �ڵ�ä�� ������ 1 �ø���.

    public void Claim()
    {
        List<OreAmount> rewards = CalculateClaimRewards();

        if (rewards.Count > 0)
        {
            GameManager.Instance.OreManager.AddRange(rewards);
            StartCoroutine(presenter.OreGainRoutine(rewards, minevisual.transform.position));
        }

        AutoMiningRuntimeData.ResetClaimTime();
    } // ���� �ڵ�ä�� ������ �����ϰ� ������ ���� �ð��� ���� �ð����� �����Ѵ�.
}