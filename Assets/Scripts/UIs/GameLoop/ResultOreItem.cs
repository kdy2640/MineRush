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
            oreNameText.text = oreAmount.oreType.ToString();
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
        multiplierText.gameObject.SetActive(true);

        multiplierText.text = $"x{multiplier:0.0}";

        multiplierText.DOKill();

        // 처음에는 조금 아래에서 시작
        multiplierText.rectTransform.anchoredPosition = new Vector2(0, 15);

        multiplierText.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Join(multiplierText.rectTransform.DOAnchorPosY(30, 0.25f));

        seq.Join(multiplierText.DOFade(1f, 0.2f));
    }
    public void HideMultiplier()
    {
        multiplierText.DOKill();

        Sequence seq = DOTween.Sequence();

        // 조금 더 위로 올라감
        seq.Join(multiplierText.rectTransform.DOAnchorPosY(40, 0.15f));

        // 사라짐
        seq.Join(multiplierText.DOFade(0f, 0.15f));

        seq.OnComplete(() =>
        {
            multiplierText.gameObject.SetActive(false);
        });
    }
    }