using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine; 
using UnityEngine.UI;

public class UI_OreAmountVisualizer : MonoBehaviour
{
    [SerializeField] private Image BackGround;
    [SerializeField] private Image OreImage;
    [SerializeField] private TextMeshProUGUI OreCount;

    [SerializeField] private float gainPopScale = 1.25f;
    [SerializeField] private float gainPopDuration = 0.12f;
    [SerializeField] private float gainSettleDuration = 0.08f;

    private Tween gainTween;

    public void SetOre(OreAmount amount)
    {
        OreImage.sprite = OreDataDB.GetOreDataSO(amount.oreType).OreSprite;
        OreCount.text = amount.amount.ToString();
    }
    public void Clear()
    { 
        OreCount.text = "";
    }
    public void PlayGain()
    {
        gainTween?.Kill();

        RectTransform countRect = OreCount.rectTransform;

        countRect.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        seq.Append(countRect.DOScale(gainPopScale, gainPopDuration).SetEase(Ease.OutBack));
        seq.Append(countRect.DOScale(1f, gainSettleDuration).SetEase(Ease.OutQuad));

        gainTween = seq;
    }
    private void OnDestroy()
    {
        gainTween?.Kill();
    }
}
