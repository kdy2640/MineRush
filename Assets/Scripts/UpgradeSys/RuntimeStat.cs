using UnityEngine;

/// <summary>
/// 게임 루프에서 실제로 사용하는 최종 스탯.
/// StatCalculator가 업그레이드 효과를 적용한 결과값이다.
/// </summary>
[System.Serializable]
public class RuntimeStat
{
    [SerializeField] private float miningPower = 1;
    [SerializeField] private float miningSpeed = 1;
    [SerializeField] private float criticalChance = 0;
    [SerializeField] private float criticalMultiplier = 2f;

    public float MiningPower => miningPower;
    public float MiningSpeed => miningSpeed;
    public float CriticalChance => criticalChance;
    public float CriticalMultiplier => criticalMultiplier;

    // 현재는 기본 스탯 값을 RuntimeStat 내부에 고정해 둔다.
    // 나중에 곡괭이 종류별 기본 스탯이 필요해지면 생성자나 별도 데이터로 기본값을 주입하도록 수정한다.
    public void Apply(StatModifier modifier, int level)
    {
        float amount = modifier.value * level;

        switch (modifier.statType)
        {
            case StatType.MiningPower:
                miningPower = ApplyValue(miningPower, modifier.modifierType, amount);
                break;

            case StatType.MiningSpeed:
                miningSpeed = ApplyValue(miningSpeed, modifier.modifierType, amount);
                break;

            case StatType.CriticalChance:
                criticalChance = ApplyValue(criticalChance, modifier.modifierType, amount);
                break;

            case StatType.CriticalMultiplier:
                criticalMultiplier = ApplyValue(criticalMultiplier, modifier.modifierType, amount);
                break;
        }
    }

    private float ApplyValue(float current, ModifierType type, float amount)
    {
        return type switch
        {
            ModifierType.Add => current + amount,
            ModifierType.Multiply => current * amount,
            _ => current
        };
    }
}
