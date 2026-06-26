using System;

public static class AutoMiningRuntimeData
{
    public static bool IsInit { get; private set; } = false;
    public static DateTime LastClaimUtc { get; private set; }

    public static void Init()
    {
        if (IsInit)
            return;
        ResetClaimTime();
    }

    public static TimeSpan GetElapsedClaimTime()
    {
        if (!IsInit)
            return TimeSpan.Zero;

        return DateTime.UtcNow - LastClaimUtc;
    }

    public static void ResetClaimTime()
    {
        IsInit = true;
        LastClaimUtc = DateTime.UtcNow;
    }
}