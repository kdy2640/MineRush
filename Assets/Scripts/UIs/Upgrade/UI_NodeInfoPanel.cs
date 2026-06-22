using TMPro;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

public class UI_NodeInfoPanel : MonoBehaviour
{
    //최종 구현에서는 아마도, 여기에서의 costText와
    //UpgradeNodePanelController의 RefreshDescriptionPanel 메서드 내부가 수정되어야 한다.
    //왜냐하면, 광물은 이미지로 나올 것이고, 가격은 그대로 숫자로 나올 거니까.

    [Header("출력할 부분")]
    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI DescriptionText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CostText { get; private set; } //이거 OreAmountText로 바꾸는 게?
    [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }

    [Header("등장 시 패널 크기")]
    [SerializeField] private float hoverScale = 1.2f;
    [Header("연출 지속시간")]
    [SerializeField] private float duration = 0.5f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Vector3 originalRotation;

    private Sequence seq;

    private void Awake()
    {
        InitTween();
    }

    private void OnEnable() //SetActive 방식으로 하셨기에 OnEnable을 써봤음.
    {
        if (seq == null)
        {
            InitTween();
        }

        seq.Restart();
    }

    private void OnDisable() //패널 비활성화 될 때
    {
        seq?.Pause();
    }

    private void InitTween()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
        originalRotation = transform.localRotation.eulerAngles;

        seq?.Kill();

        seq = DOTween.Sequence().SetAutoKill(false).Pause();
        seq.Join(rectTransform.DOScale(originalScale, duration).From(originalScale * hoverScale).SetEase(Ease.OutCubic))
            .Join(rectTransform.DOPunchRotation(Vector3.forward * 15f, duration, 6, 0.5f))
            .OnStart(() =>
            {
                rectTransform.localRotation = Quaternion.Euler(originalRotation);
            });
    } // 설명창이 켜질 때 재생할 트윈을 초기화하는 함수.
    // 자기 자신을 여기서 끄지 않고, 처음 숨기는 처리는 UpgradeNodePanelController에서 담당한다.

    public void SetInfo(UpgradeState upgradeState)
    {
        if (upgradeState == null) return;

        DisplayNameText.text = upgradeState.data.displayName;
        DescriptionText.text = UpgradeDescriptionFormatter.GetDescription(upgradeState);

        bool isMaxLevel = GameManager.Instance.Upgrade.IsMaxLevel(upgradeState);

        if (isMaxLevel)
        {
            LevelText.text = "<color=#6A4CFF>Level : 최대 레벨</color>";
            CostText.text = "<color=#6A4CFF>최대 레벨</color>";
            return;
        }

        string costText = "";
        LevelText.text = $"Level : {upgradeState.level} / {upgradeState.data.maxLevel}";

        foreach (OreAmount oreAmount in upgradeState.GetCurrentCost())
        {
            bool isEnough = GameManager.Instance.OreManager.HasEnoughOre(oreAmount);
            int currentAmount = GameManager.Instance.OreManager.GetAmount(oreAmount.oreType);
            string icon = GetOreIconTag(oreAmount.oreType);
            string textColor = isEnough ? "#00FF00" : "#FF4444";

            costText += $"{icon} : <color={textColor}>{currentAmount} / {oreAmount.amount}</color>\n";
        }

        CostText.text = costText.TrimEnd();
    }
    private string GetOreIconTag(OreType oreType)
    {
        string spriteName = oreType switch
        {
            OreType.Copper => "CopperOre",
            OreType.Iron => "IronOre",
            OreType.Gold => "GoldOre",
            OreType.Diamond => "DiamondOre",
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(spriteName))
        {
            return string.Empty;
        }

        return $"<sprite name=\"{spriteName}\">";
    }
}