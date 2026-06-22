using System;
using UnityEngine;

public class RuntimeXPProvider :
    MonoBehaviour,
    IXPProvider
{
    public event Action<int, int> OnXPChanged;
    private void Awake()
    {
        Debug.Log("RuntimeXPProvider Awake");
    }

    private void OnEnable()
    {
        Debug.Log("RuntimeXPProvider Enable");

        if (XPSystem.Instance != null)
        {
            XPSystem.Instance.OnXPChanged += ForwardXPChanged;
        }
    }

    private void OnDisable()
    {
        if (XPSystem.Instance != null)
        {
            XPSystem.Instance.OnXPChanged -= ForwardXPChanged;
        }
    }

    private void ForwardXPChanged(
        int current,
        int max)
    {
        Debug.Log( $"PROVIDER FORWARD : {current}/{max}");

        OnXPChanged?.Invoke(
            current,
            max);
    }

    public int GetCurrentXP()
    {
        if (XPSystem.Instance == null)
            return 0;

        return XPSystem.Instance.GetLevelXP();
    }

    public int GetRequiredXP()
    {
        if (XPSystem.Instance == null)
            return 100;

        return XPSystem.Instance.GetRequiredXP();
    }
}