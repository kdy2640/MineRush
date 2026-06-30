using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultOreItem : MonoBehaviour
{
    [Header("[ UI References ]")]
    [SerializeField] private Image oreIcon;
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI amountText;
    //배수(X5등) 표시용 텍스트
    [SerializeField] private TextMeshProUGUI multiplierText;

    [Header("[ Ore Icons ]")]
    [SerializeField] private Sprite copperSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite diamondSprite;
    public void SetData(OreAmount oreAmount)
    {
            oreNameText.text = OreTextFormatter.GetDisplayName(oreAmount.oreType);
            amountText.text = $"x{oreAmount.amount}";

        switch (oreAmount.oreType)
            {
            case OreType.Copper:
                oreIcon.color = new Color(0.8f, 0.5f, 0.2f); break;

            case OreType.Iron:
                oreIcon.color = Color.gray; break;

            case OreType.Gold:
                oreIcon.color = Color.yellow; break;

            case OreType.Diamond:
                oreIcon.color = Color.cyan; break;
            }
    }
    //수량 텍스트만 변경
    public void SetAmount(int amount)
    {
        amountText.text = $"x{amount}";
    }
    public void ShowMultiplier(float multiplier)
    {
        multiplierText.DOKill();

        multiplierText.text = $"×{multiplier:0.00}";

        RectTransform rect = multiplierText.rectTransform;

        rect.localScale = Vector3.zero;

        multiplierText.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Join(multiplierText.DOFade(1f, 0.2f));

        seq.Join(rect.DOScale(1.25f, 0.18f).SetEase(Ease.OutBack));

        seq.Append(rect.DOScale(1f, 0.12f));
    }
    public void HideMultiplier()
    {
        multiplierText.DOKill();

        RectTransform rect = multiplierText.rectTransform;

        Sequence seq = DOTween.Sequence();

        seq.Join(multiplierText.DOFade(0f, 0.3f));

        seq.Join(rect.DOScale(0.8f, 0.3f));
    }
    }