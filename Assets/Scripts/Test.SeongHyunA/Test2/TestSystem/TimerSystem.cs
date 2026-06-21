
using System;
using UnityEngine;

public class TimerSystem : MonoBehaviour, ITimerProvider
{
    public static TimerSystem Instance;

    public event Action<float> OnTick;

    [SerializeField]
    private float sessionDuration = 10f;

    private float currentTime;

    private bool running;

    [SerializeField]
    private GameFlowController flow;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        currentTime = sessionDuration;

        running = true;

        OnTick?.Invoke(currentTime);
    }

    public void StopTimer()
    {
        running = false;
    }

    private void Update()
    {
        if (!running)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        OnTick?.Invoke(currentTime);

        if (currentTime <= 0)
        {
            running = false;

            flow.EndGame();
        }
    }

    public float GetTime()
    {
        return currentTime;
    }
}