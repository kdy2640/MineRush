using UnityEngine;

public static class MiningCalculator
{
    public static float CalculateDamage()
    {
        RuntimeStat stat = GameManager.Instance.Upgrade.GetRuntimeStat();

        float random = Random.value;

        // 예: 20% 확률로 아이템 지급 (0.2 이하)
        if (random <= stat.CriticalChance)
        {
            return stat.MiningPower * stat.CriticalMultiplier;
        }
        else
        {
            return stat.MiningPower;
        }
    }
}
