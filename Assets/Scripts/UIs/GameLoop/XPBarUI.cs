using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private TMP_Text levelText;

    [SerializeField] private TMP_Text progressText;

    [SerializeField] private Image fill;

    [SerializeField] private Color normalColor;

    [SerializeField] private Color fullColor;

#if UNITY_EDITOR

    [SerializeField]
    private bool debugMode = true;

    private float debugValue;

#endif

    private void Update()
    {
#if UNITY_EDITOR

        if (!debugMode) return;

        debugValue += Time.deltaTime * 30f;

        SetXP(debugValue % 100f, 100f);

#endif
    }

    public void SetXP( float currentXP, float maxXP)
    {
        slider.maxValue = maxXP;
        slider.value = Mathf.Clamp(currentXP, 0, maxXP);

        progressText.text = $"{currentXP:0} / {maxXP:0}";

        float ratio = currentXP / maxXP;

        bool isFull = ratio >= 0.9f;

        if (fill != null)
        {
            fill.color = isFull ? fullColor : normalColor;
        }
    }

    public void SetData( int level, float currentXP, float requiredXP)
    {
        levelText.text = $"Lv.{level}";

        progressText.text =
            $"{currentXP:0}/{requiredXP:0}";

        slider.maxValue = requiredXP;

        slider.value = currentXP;
    }
}