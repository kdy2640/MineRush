using System.Collections.Generic;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    [SerializeField] private UpgradeData autoMiningUpgradeData;
    // 인스펙터에서 자동채굴 전용 UpgradeData SO를 넣는다.

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
    // 캐싱해두기. 아마 작동 될듯.

    private void Start()
    {
        AutoMiningRuntimeData.Init();
        state = GameManager.Instance.Upgrade.GetState(autoMiningUpgradeData);
    } // 자동채굴 시간 데이터가 초기화되지 않았다면 현재 시간으로 초기화한다.

    public int GetLevel()
    {
        return State.level;
    } // 현재 자동채굴 업그레이드 레벨을 반환한다.
    // ui에도 현재레벨 표사에 쓰일 수도?

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
    } // 현재 레벨에서 다음 레벨로 올릴 때 필요한 자동채굴 비용을 계산한다.

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
    } // 현재 레벨 기준 10초당 보상에 완료된 10초 단위 횟수를 곱해 실제 받을 보상을 계산한다.

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
    } // 기존 누적 보상을 먼저 정산한 뒤, 비용을 지불하고 자동채굴 레벨을 1 올린다.

    public void Claim()
    {
        List<OreAmount> rewards = CalculateClaimRewards();

        if (rewards.Count > 0)
        {
            GameManager.Instance.OreManager.AddRange(rewards);
        }

        AutoMiningRuntimeData.ResetClaimTime();
    } // 계산된 자동채굴 보상을 지급하고 마지막 수령 시간을 현재 시간으로 갱신한다.
}