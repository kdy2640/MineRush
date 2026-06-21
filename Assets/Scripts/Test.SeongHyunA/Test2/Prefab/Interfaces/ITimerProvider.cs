using System;

public interface ITimerProvider
{
    event Action<float> OnTick;

    float GetTime();
}