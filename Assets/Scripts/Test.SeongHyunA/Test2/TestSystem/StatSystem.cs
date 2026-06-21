using UnityEngine;

public class StatSystem : MonoBehaviour
{
    public static StatSystem Instance;

    [SerializeField] private RuntimeStat runtimeStat = new RuntimeStat();

    private void Awake()
    {
        Instance = this;
    }

    public RuntimeStat GetStat() => runtimeStat;

    public void ApplyModifier(StatModifier modifier, int level)
    {
        runtimeStat.Apply(modifier, level);
    }
}