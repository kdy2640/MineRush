using System;
using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    public static TimerSystem Instance;

    public event Action<float> OnTick;

    [Header("Session Duration")]
    [SerializeField] private float sessionDuration = 10f;

    private float currentTime;

    private bool running;

    [SerializeField] private GameFlowController flow;

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

    public void ResetTimer()
    {
        running = false;

        currentTime = sessionDuration;

        OnTick?.Invoke(currentTime);
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
            Debug.Log("TIMER END");

            running = false;

            if (flow != null)
                flow.EndGame();
        }
    }

    public float GetTime()
    {
        return currentTime;
    }
}