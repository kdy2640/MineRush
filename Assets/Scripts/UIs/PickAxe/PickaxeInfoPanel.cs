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
    [field: SerializeField] public GameObject RightMouseGuidePanel { get; private set; }
    [SerializeField] private RectTransform targetLayoutRoot;


    //전달받은 데이터를 실제로 전시하는 역할.
    public void SetInfo(PickaxesDataSO currentPickaxe)
    {
        if (currentPickaxe == null)
            return;
        int runtimeTier = GameManager.Instance.Upgrade.GetRuntimeStat().PickaxeTier;
        bool isUnlocked = currentPickaxe.Tier <= runtimeTier;

        if (isUnlocked)
        {
            DisplayNameText.text = currentPickaxe.DisplayName;
            MiningPowerText.gameObject.SetActive(true);
            MiningSpeedText.gameObject.SetActive(true);
            MiningRadiusText.gameObject.SetActive(true);
            CriticalChanceText.gameObject.SetActive(true);
            MiningPowerText.text = $"채굴 공격력 : {currentPickaxe.MiningPower}";
            MiningSpeedText.text = $"채굴속도 : {currentPickaxe.MiningSpeed}";
            MiningRadiusText.text = $"채굴반경 : {currentPickaxe.MiningRadius}";
            CriticalChanceText.text = $"크리티컬 확률 : {currentPickaxe.CriticalChance * 100f:0.##}%";
        }
        else
        {
            DisplayNameText.text = "??????";
            MiningPowerText.gameObject.SetActive(false);
            MiningSpeedText.gameObject.SetActive(false);
            MiningRadiusText.gameObject.SetActive(false);
            CriticalChanceText.gameObject.SetActive(false);
        }
        
        if (CostText != null && CostTitle != null)
        {
            RefreshCostText(currentPickaxe);
        } // 현재 장착중인 곡갱이 패널은 이게 없으므로 예외처리
    }

    public void RefreshCostText(PickaxesDataSO currentPickaxe)
    {
        UpgradeState state = GameManager.Instance.Upgrade.GetState(currentPickaxe.UpgradeData);
        int runtimeTier = GameManager.Instance.Upgrade.GetRuntimeStat().PickaxeTier;
        CostText.gameObject.SetActive(false);
        RightMouseGuidePanel.SetActive(false);
        if (currentPickaxe.Tier == runtimeTier)
        {
            CostTitle.text = "장착 중인 현재 곡갱이";
        }
        else if (currentPickaxe.Tier < runtimeTier)
        {
            CostTitle.text = "이미 해금했습니다.";
        }
        else if (currentPickaxe.Tier > runtimeTier + 1)
        {
            CostTitle.text = "이전 곡갱이를 먼저 해금해주세요.";
        }
        else
        {
            CostTitle.gameObject.SetActive(true);
            CostTitle.text = "비용 :";
            CostText.gameObject.SetActive(true);
            CostText.text = UpgradeOreCostTextFormatter.GetAllOreCostText(
                state.GetCurrentCost(),
                GameManager.Instance.OreManager
            );
            RightMouseGuidePanel.SetActive(true);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(targetLayoutRoot);
    }
}
