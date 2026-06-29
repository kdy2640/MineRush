using UnityEngine;

public static class MiningCalculator
{
    public static float CalculateDamage(MiningType type)
    {
        RuntimeStat stat = GameManager.Instance.Upgrade.GetRuntimeStat();
        float damage = 0f;
        switch (type)
        {
            case MiningType.Pickaxe:
                damage = stat.MiningPower;
                break;
            case MiningType.Laser:
                damage = stat.LaserDamage; 
                break;
            case MiningType.Bomb:
                damage = stat.BombDamage; 
                break;
            case MiningType.Length:
                break;
        }
        return damage;
    }
    public static float CalculateInternalDamage(RuntimeStat stat, float damage)
    {
         
        float random = Random.value;
         
        if (random <= stat.CriticalChance)
        {
            return damage * stat.CriticalMultiplier;
        }
        else
        {
            return damage;
        }
    }
}
