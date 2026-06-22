using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private MonoBehaviour providerObject;

    private ITimerProvider provider;

    private void Awake()
    {
        provider = providerObject as ITimerProvider;
    }

    private void OnEnable()
    {
        if (provider == null) return;

        provider.OnTick += UpdateUI;

        UpdateUI( provider.GetTime() );
    }

    private void OnDisable()
    {
        if (provider == null)
            return;

        provider.OnTick -= UpdateUI;
    }

    private void UpdateUI(float t)
    {
        text.text = t.ToString("F2");

        if (t <= 1f)
            text.color = Color.red;
        else
            text.color = Color.white;
    }
}