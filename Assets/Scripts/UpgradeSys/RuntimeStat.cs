using UnityEngine;

/// <summary>
/// 루프 씬에 넘겨주기 전 최종 스탯을 의미.
/// </summary>
public class RuntimeStat
{
    public float MiningPower { get; private set; } = 1;
    public float MiningSpeed { get; private set; } = 1;
    public float CriticalChance { get; private set; } = 0;
    public float CriticalMultiplier { get; private set; } = 2f;
    // 여기에 base값이 박혀있는 이유.
    // 어차피 계산기에서 객체 생성하고 값 뱉어주고 객체 없앰.
    // TODO 나중에 곡갱이 종류를 추가해서 base 값을 바꾸고 싶으면 고치기
    
    public void Apply(StatModifier modifier, int level)
    {
        float amount = modifier.value * level;

        switch (modifier.statType)
        {
            case StatType.MiningPower:
                MiningPower = ApplyValue(MiningPower, modifier.modifierType, amount);
                break;

            case StatType.MiningSpeed:
                MiningSpeed = ApplyValue(MiningSpeed, modifier.modifierType, amount);
                break;

            case StatType.CriticalChance:
                CriticalChance = ApplyValue(CriticalChance, modifier.modifierType, amount);
                break;

            case StatType.CriticalMultiplier:
                CriticalMultiplier = ApplyValue(CriticalMultiplier, modifier.modifierType, amount);
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
