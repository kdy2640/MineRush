using System;
using UnityEngine;

/// <summary>
/// 업그레이드로 변경할 수 있는 스탯 종류.
/// </summary>
public enum StatType
{
    MiningPower,
    MiningSpeed,

    CriticalChance,
    CriticalMultiplier,
}

/// <summary>
/// 스탯을 더할지, 곱할지 나타내는 변경 방식.
/// </summary>
public enum ModifierType
{
    Add,
    Multiply
}

/// <summary>
/// 업그레이드 효과 1개를 나타내는 데이터.
/// 어떤 스탯을, 어떤 방식으로, 얼마나 바꿀지 정의한다. <br/><br/>
/// 예시)<br/>
/// [업그레이드]<br/>
/// 채굴 속도 + 1  &lt;&lt;&lt; 이 스탯 하나를 의미.<br/>
/// 채굴 범위 + 1<br/>
/// [재료]<br/>
/// 철 - 10개<br/>
/// 구리 - 10개<br/>
/// </summary>
[Serializable]
public class StatModifier
{
    public StatType statType;
    public ModifierType modifierType;
    public float value;
}
