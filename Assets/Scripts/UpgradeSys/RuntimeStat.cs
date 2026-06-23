using UnityEngine;

/// <summary>
/// 게임 루프에서 실제로 사용하는 최종 스탯.
/// StatCalculator가 업그레이드 효과를 적용한 결과값이다.
/// </summary>
[System.Serializable]
public class RuntimeStat
{
    public RuntimeStat()
    {
        Initialize();
    }

    private void Initialize()
    {
        pickaxeTier = 0f;
        miningPower = 1f;
        miningSpeed = 1f;
        miningRadius = 1f;
        criticalChance = 0f;
        criticalMultiplier = 2f;
        extraDuration = 0f;
        rewardMultiplier = 1f;
        maxOreTier = 1f;
        stoneCount = 20;
        OreFragmentChanceArr = new float[(int)OreType.Length];
        OrePureChanceArr = new float[(int)OreType.Length];
    }
    [Header("채굴 스탯")]
    [SerializeField] private float pickaxeTier = 0f;
    // 1회 채굴/공격 시 적용되는 기본 채굴력. 
    [SerializeField] private float miningPower = 1f;

    // 1초에 공격할 횟수
    [SerializeField] private float miningSpeed = 1f;

    // 채굴 범위.   1~
    [SerializeField] private float miningRadius = 1f;

    // 치명타 확률.범위 [0,1]
    // 예: 0.1 = 10%, 0.25 = 25%, 1 = 100%.
    [SerializeField] private float criticalChance = 0f;

    // 치명타 대미지 배율.
    // 예 : 2 = 200% 대미지, 1.5 = 150% 대미지.
    [SerializeField] private float criticalMultiplier = 2f;

    [Header("게임 진행 스탯")]
    // 추가 지속시간.
    // 예: 1.5 = 지속시간 1.5초 증가.
    [SerializeField] private float extraDuration = 0f;

    // 보상 배율.
    // 예: 1 = 기본 보상, 1.5 = 보상 150%, 2 = 보상 200%.
    [SerializeField] private float rewardMultiplier = 1f;


    [Header("광석 스탯")]
    // 광석 최대 티어. 내부 계산은 float로 하지만, 실제 사용 시에는 정수 티어로 변환. 
    [SerializeField] private float maxOreTier = 0f;

    // 광석 최대 개수. 내부 계산은 float로 하지만, 실제 사용 시에는 정수 티어로 변환. 
    [SerializeField] private float stoneCount = 10f;

    // 광석 조각 확률
    [SerializeField] private float[] OreFragmentChanceArr = new float[(int)OreType.Length];

    // 광석 순수 확률
    [SerializeField] private float[] OrePureChanceArr = new float[(int)OreType.Length];

    public int PickaxeTier => Mathf.Max(0, Mathf.RoundToInt(pickaxeTier));
    public int MaxOreTier => Mathf.Max(0, Mathf.RoundToInt(maxOreTier));
    public int StoneCount => Mathf.Max(0, Mathf.RoundToInt(stoneCount));
    public float MiningPower => miningPower;
    public float MiningSpeed => miningSpeed;
    public float MiningRadius => miningRadius;
    public float CriticalChance => criticalChance;
    public float CriticalMultiplier => criticalMultiplier;
    public float ExtraDuration => extraDuration;
    public float RewardMultiplier => rewardMultiplier;

    //
    public void ApplyPickaxe(PickaxesDataSO pickSO)
    { 
        miningPower += pickSO.MiningPower;
        miningSpeed += pickSO.MiningSpeed;
        miningRadius += pickSO.MiningRadius;
        criticalChance += pickSO.CriticalChance;
    }

    // 현재는 기본 스탯 값을 RuntimeStat 내부에 고정해 둔다.
    // 나중에 곡괭이 종류별 기본 스탯이 필요해지면 생성자나 별도 데이터로 기본값을 주입하도록 수정한다.
    public void Apply(StatModifier modifier, int level)
    {
        float amount = modifier.value * level;
        int oreIndex = 0;
        switch (modifier.statType)
        {
            case StatType.PickaxeTier:
                pickaxeTier = ApplyValue(pickaxeTier, modifier.modifierType, amount);
                break;
            case StatType.MiningPower:
                miningPower = ApplyValue(miningPower, modifier.modifierType, amount);
                break;

            case StatType.MiningSpeed:
                miningSpeed = ApplyValue(miningSpeed, modifier.modifierType, amount);
                break;

            case StatType.MiningRadius:
                miningRadius = ApplyValue(miningRadius, modifier.modifierType, amount);
                break;

            case StatType.CriticalChance:
                criticalChance = ApplyValue(criticalChance, modifier.modifierType, amount);
                break;

            case StatType.CriticalMultiplier:
                criticalMultiplier = ApplyValue(criticalMultiplier, modifier.modifierType, amount);
                break;

            case StatType.ExtraDuration:
                extraDuration = ApplyValue(extraDuration, modifier.modifierType, amount);
                break;

            case StatType.RewardMultiplier:
                rewardMultiplier = ApplyValue(rewardMultiplier, modifier.modifierType, amount);
                break;
            case StatType.MaxOreTier:
                maxOreTier = ApplyValue(maxOreTier, modifier.modifierType, amount);
                break;
            case StatType.StoneCount: 
                stoneCount = ApplyValue(stoneCount, modifier.modifierType, amount);
                break;
            case StatType.FragChance:
                oreIndex = (int)modifier.oreType;
                OreFragmentChanceArr[oreIndex] = ApplyValue(OreFragmentChanceArr[oreIndex], modifier.modifierType, amount);
                break;
            case StatType.PureChance:
                oreIndex = (int)modifier.oreType; 
                OrePureChanceArr[oreIndex] = ApplyValue(OrePureChanceArr[oreIndex], modifier.modifierType, amount);
                break;
        }
    }

    private float ApplyValue(float current, ModifierType type, float amount)
    {
        return type switch
        {
            ModifierType.Add => current + amount,

            // amount 0.2 -> 1.2배
            ModifierType.Multiply => current * (1f + amount),

            // 곡갱이 티어의 스탯 또는 광물티어용
            ModifierType.Max => Mathf.Max(current, amount),
            
            _ => current
        };
    }
    public float GetOreFragmentChance(OreType ore)
    {
        if (ore == OreType.None || ore == OreType.Length) return -1;
        return OreFragmentChanceArr[(int)ore];
    }
    public float GetOrePureChance(OreType ore)
    {
        if (ore == OreType.None || ore == OreType.Length) return -1;
        return OrePureChanceArr[(int)ore];
    }
}