using System;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Color normalColor = Color.white;

    [SerializeField] private Color dangerColor = Color.red;

    public event Action OnTimerEnded;

    private bool ended;

    public void SetTime(float time)
    {
        time = Mathf.Max(0, time);

        timerText.text = $"{time:00.00}";

        timerText.color = time <= 1f ? dangerColor : normalColor;

        if (!ended && time <= 0f)
        {
            ended = true;
            OnTimerEnded?.Invoke();
        }
    }

    public void ResetTimer()
    {
        ended = false;
    }
}