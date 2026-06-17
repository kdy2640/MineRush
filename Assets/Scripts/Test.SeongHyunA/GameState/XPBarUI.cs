using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private int maxXP = 100;

    public void SetXP(int value)
    {
        float target = Mathf.Clamp01((float)value / maxXP);

        slider.DOValue(target, 0.8f)
            .SetEase(Ease.OutCubic);
    }

    public void ResetXP()
    {
        slider.value = 0f;
    }
}