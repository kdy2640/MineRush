using UnityEngine;

public static class MiningCalculator
{
    public static float CalculateDamage()
    {
        RuntimeStat stat = GameManager.Instance.Upgrade.GetRuntimeStat();

        float random = Random.value;
         
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
