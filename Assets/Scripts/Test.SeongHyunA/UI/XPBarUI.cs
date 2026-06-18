using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    [SerializeField] private float maxXP = 100f;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color maxColor = Color.purple;
    public void SetXP(int value)
    {
        if (slider == null) return;

        float targetRatio = Mathf.Clamp01((float)value / maxXP);

        slider.DOKill();

        slider.DOValue(targetRatio, 0.8f).SetEase(Ease.OutCubic)
            .OnComplete(()=>
            { CheckMaxXp(); });
    }
    private void CheckMaxXp()
    {
        if(slider.value >=1f)
        {
            fillImage.DOColor(maxColor, 0.3f);
        }
        else
        {
            fillImage.DOColor(normalColor, 0.3f);
        }
    }
    public void ResetXP()
    {
        slider.value = 0f;

        if (fillImage != null)
        {
            fillImage.color = normalColor;
        }
    }
    
}