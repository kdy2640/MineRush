using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
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
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
        originalRotation = transform.localRotation.eulerAngles;
        gameObject.SetActive(false);
        
        seq = DOTween.Sequence().SetAutoKill(false).Pause();
        seq.Join(rectTransform.DOScale(originalScale, duration).From(originalScale * hoverScale).SetEase(Ease.OutCubic))
            .Join(rectTransform.DOPunchRotation(Vector3.forward * 15f, duration, 6, 0.5f))
            .OnStart(() =>
            {
                rectTransform.localRotation = Quaternion.Euler(originalRotation);
            });
    }

    private void OnEnable() //SetActive 방식으로 하셨기에 OnEnable을 써봤음.
    {
        seq.Restart();
    }
    private void OnDisable() //패널 비활성화 될 때
    {
    }

    public void SetInfo(UpgradeState upgradeState)
    {
        if (upgradeState == null) return;

        DisplayNameText.text = upgradeState.data.displayName; 

        // DescriptionText.text = upgradeState.data.description; 
        //
        // LevelText.text = $"Level : {upgradeState.level} / {upgradeState.data.maxLevel}";

        string costText = ""; 

        foreach (OreAmount oreAmount in upgradeState.GetCurrentCost()) 
        {
            costText += $"{oreAmount.oreType} : {oreAmount.amount}\n";
        }
 
        CostText.text = costText; 
    }
}
