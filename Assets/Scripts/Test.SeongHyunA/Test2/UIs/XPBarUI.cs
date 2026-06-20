using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image fill;

    private void Start()
    {
        Invoke(nameof(Register), 0.1f);
    }

    private void Register()
    {
        if (XPSystem.Instance == null)
            return;

        XPSystem.Instance.OnXPChanged += UpdateXP;

        UpdateXP(
            XPSystem.Instance.GetLevelXP(),
            XPSystem.Instance.GetRequiredXP());
    }

    private void OnDestroy()
    {
        if (XPSystem.Instance != null)
            XPSystem.Instance.OnXPChanged -= UpdateXP;
    }

    private void UpdateXP(
        int current,
        int max)
    {
        float value =
            (float)current / max;

        slider.value = value;

        if (value >= 1f)
            fill.color = Color.magenta;
        else
            fill.color = Color.green;
    }
}