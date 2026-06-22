using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private Image fill;

    [SerializeField] private TMP_Text xpText;

    [SerializeField] private MonoBehaviour providerObject;

    private IXPProvider provider;

    private void Awake()
    {
        provider = providerObject as IXPProvider;
    }

    private void OnEnable()
    {
        if (provider == null) return;

        provider.OnXPChanged += UpdateXP;

        UpdateXP(
            provider.GetCurrentXP(),
            provider.GetRequiredXP());
    }

    private void OnDisable()
    {
        if (provider == null) return;

        provider.OnXPChanged -= UpdateXP;
    }

    private void UpdateXP( int current, int max)
    {
        float value = (float)current / max;

        slider.value = value;

        if (xpText != null)
            xpText.text = $"{current} / {max}";

        if (value >= 0.9f)
            fill.color = Color.purple;
        else
            fill.color = Color.green;
    }
}