using System;
using UnityEngine;
/// <summary>
/// 스탯의 종류
/// </summary>
public enum StatType
{
    MiningPower,
    MiningSpeed,
    
    CriticalChance,
    CriticalMultiplier,
    MoveSpeed,
}
/// <summary>
/// 스탯이 더하기인지 곱하기인지 나타냄.
/// </summary>
public enum ModifierType
{
    Add,
    Multiply
}
/// <summary>
/// 스탯 변경 1개를 의미함.<br></br>
/// 예시)<br></br>
/// [업그레이드]<br></br>
/// 채굴 속도 + 1  &lt;&lt;&lt; 이 스탯 하나를 의미.<br></br>
/// 채굴 범위 + 1<br></br>
/// [재료]<br></br>
/// 철 - 10개<br></br>
/// 구리 - 10개<br></br>
/// </summary>
[Serializable]
public class StatModifier
{
    public StatType statType;
    public ModifierType modifierType;
    public float value;
}