using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    private Action OnDecommission;
    private Action<Poolable> OnReturn;
    public abstract void Initialize(object obj);
    public abstract void ResetState();
    public void RequestReturn()
    {
        OnReturn?.Invoke(this);
        OnDecommission?.Invoke();
    }
    public void SubscribeDecommissionListener(Action ac)
    {
        OnDecommission += ac;
    }

    public void UnSubscribeDecommissionListener(Action ac)
    {
        OnDecommission -= ac;
    }
    public void SubscribeReturnListener(Action<Poolable> ac)
    {
        OnReturn += ac;
    }

    public void UnSubscribeReturnListener(Action<Poolable> ac)
    {
        OnReturn -= ac;
    }
}
