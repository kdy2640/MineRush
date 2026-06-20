using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.OnTick += UpdateUI;
            UpdateUI(0);
        }
    }

    private void OnDestroy()
    {
        if (TimerSystem.Instance != null)
            TimerSystem.Instance.OnTick -= UpdateUI;
    }
    private void OnEnable()
    {
        if (TimerSystem.Instance != null)
            TimerSystem.Instance.OnTick += UpdateUI;
    }

    private void OnDisable()
    {
        if (TimerSystem.Instance != null)
            TimerSystem.Instance.OnTick -= UpdateUI;
    }

    private void UpdateUI(float t)
    {
        text.text = t.ToString("F2");
    }
}