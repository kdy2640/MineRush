using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickaxeInfoPanel : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningPowerText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningSpeedText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningRadiusText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CriticalChanceText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CostTitle { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CostText { get; private set; }


    //전달받은 데이터를 실제로 전시하는 역할.
    public void SetInfo(PickaxesDataSO currentPickaxe)
    {
        if (currentPickaxe == null)
            return;

        DisplayNameText.text = currentPickaxe.DisplayName;
        MiningPowerText.text = $"채굴 공격력 : {currentPickaxe.MiningPower}";
        MiningSpeedText.text = $"채굴속도 : {currentPickaxe.MiningSpeed}";
        MiningRadiusText.text = $"채굴반경 : {currentPickaxe.MiningRadius}";
        CriticalChanceText.text = $"크리티컬 확률 : {currentPickaxe.CriticalChance}";
        if (CostText != null && CostTitle != null)
        {
            RefreshCostText(currentPickaxe);
        } // 현재 장착중인 곡갱이 패널은 이게 없으므로 예외처리
    }

    public void RefreshCostText(PickaxesDataSO currentPickaxe)
    {
        UpgradeState state = GameManager.Instance.Upgrade.GetState(currentPickaxe.UpgradeData);
        if (currentPickaxe.Tier != GameManager.Instance.Upgrade.GetRuntimeStat().PickaxeTier + 1)
        {
            CostTitle.gameObject.SetActive(false);
            CostText.gameObject.SetActive(false);
            return;
        } // 이미 구매함.
        else
        {
            CostTitle.gameObject.SetActive(true);
            CostText.gameObject.SetActive(true);
            CostText.text = UpgradeOreCostTextFormatter.GetAllOreCostText(
                state.GetCurrentCost(),
                GameManager.Instance.OreManager
            );
        }
    }
}
