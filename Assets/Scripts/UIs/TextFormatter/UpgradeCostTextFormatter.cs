using System.Collections.Generic;

public static class UpgradeOreCostTextFormatter
{
    public static string GetAllOreCostText(UpgradeState upgradeState, OreManager oreManager)
    {
        if (upgradeState == null || oreManager == null)
        {
            return string.Empty;
        }

        var currentCosts = upgradeState.GetCurrentCost();

        if (currentCosts == null || currentCosts.Count == 0)
        {
            return string.Empty;
        }

        List<string> costTexts = new();

        foreach (OreAmount oreAmount in currentCosts)
        {
            costTexts.Add(GetOreCostLine(oreAmount, oreManager));
        }

        return string.Join("\n", costTexts).TrimEnd();
    } // 업그레이드에 필요한 광물 비용들을 설명창에 표시할 문자열로 만들어주는 함수.

    private static string GetOreCostLine(OreAmount oreAmount, OreManager oreManager)
    {
        bool isEnough = oreManager.HasEnoughOre(oreAmount);
        int currentAmount = oreManager.GetAmount(oreAmount.oreType);

        string icon = OreTextFormatter.GetTmpTag(oreAmount.oreType);
        string textColor = isEnough ? "#00FF00" : "#FF4444";

        return $"{icon} : <color={textColor}>{currentAmount} / {oreAmount.amount}</color>";
    } // 광물 하나의 현재 보유량 / 필요량을 TMP 문자열로 만들어주는 함수.
}