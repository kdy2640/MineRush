using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Color normalColor = Color.white;

    [SerializeField] private Color dangerColor = Color.red;

    public event Action OnTimerEnded;

    private bool ended;

    private Tween pulseTween;

    public void SetTime(float time)
    {
        time = Mathf.Max(0, time);

        timerText.text = $"{time:00.00}";

        if (time <= 2f)
        {
            timerText.color = dangerColor;

            if (pulseTween == null)
            {
                pulseTween = timerText.rectTransform
                    .DOScale(1.15f, 0.2f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
        else
        {
            timerText.color = normalColor;
        }

        if (!ended && time <= 0f)
        {
            ended = true;

            pulseTween?.Kill();

            OnTimerEnded?.Invoke();
        }
    }

    public void ResetTimer()
    {
        ended = false;

        pulseTween?.Kill();

        pulseTween = null;

        timerText.rectTransform.localScale = Vector3.one;
    }
}