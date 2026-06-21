using System.Collections.Generic;
using UnityEngine;

public static class UpgradeDescriptionFormatter
{
    private enum ValueFormat
    {
        Number,
        Int,
        Percent,
        Second
    } // 값에 따라 어떤걸 나타낼지 위한 enum

    public static string GetDescription(UpgradeState upgradeState)
    {
        if (upgradeState == null || upgradeState.data == null)
        {
            return string.Empty;
        }

        List<StatModifier> statModifiers = upgradeState.data.statModifiers;

        if (statModifiers == null || statModifiers.Count == 0)
        {
            return string.Empty;
        }

        List<string> descriptions = new();
        bool isMaxLevel = upgradeState.level >= upgradeState.data.maxLevel;

        foreach (StatModifier modifier in statModifiers)
        {
            descriptions.Add(GetStatDescription(modifier, upgradeState.level, isMaxLevel));
        }

        return string.Join("\n\n", descriptions);
    } // 업그레이드 상태를 받아서 설명창에 표시할 전체 설명 문장을 만들어주는 함수.

    private static string GetStatDescription(StatModifier modifier, int level, bool isMaxLevel)
    {
        return modifier.statType switch
        {
            StatType.PickaxeTier =>
                "잘못된 스탯이 노드에 들어가있습니다",

            StatType.MiningPower =>
                GetValueDescription("채굴 데미지 :\n", modifier.value, level, isMaxLevel, ValueFormat.Number),

            StatType.MiningSpeed =>
                GetValueDescription("채굴 속도 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            StatType.MiningRadius =>
                GetValueDescription("채굴 범위 :\n", modifier.value, level, isMaxLevel, ValueFormat.Int),

            StatType.CriticalChance =>
                GetValueDescription("치명타 확률 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            StatType.CriticalMultiplier =>
                GetValueDescription("치명타 피해량 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            StatType.ExtraDuration =>
                GetValueDescription("채굴 시간 :\n", modifier.value, level, isMaxLevel, ValueFormat.Second),

            StatType.RewardMultiplier =>
                GetValueDescription("보상 증가 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            StatType.MaxOreTier =>
                GetOreTierDescription(modifier.value, isMaxLevel),

            StatType.StoneCount =>
                GetValueDescription("시작 광석 수 :\n", modifier.value, level, isMaxLevel, ValueFormat.Int),

            StatType.FragChance =>
                GetValueDescription("파편 광석 확률 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            StatType.PureChance =>
                GetValueDescription("순수 광석 확률 :\n", modifier.value, level, isMaxLevel, ValueFormat.Percent),

            _ =>
                "알 수 없는 스탯입니다"
        };
    } // StatType에 따라 어떤 설명 문장을 만들지 정하는 함수.

    private static string GetValueDescription(string statName, float valuePerLevel, int level, bool isMaxLevel, ValueFormat unit)
    {
        float currentValue = valuePerLevel * level;
        float nextValue = valuePerLevel * (level + 1);

        if (isMaxLevel)
        {
            return $"{statName} +{FormatValue(currentValue, unit)}";
        }

        return $"{statName} +{FormatValue(currentValue, unit)} -> +{FormatValue(nextValue, unit)}";
    } // 일반 수치형 스탯 설명을 만들어주는 함수.

    private static string GetOreTierDescription(float value, bool isMaxLevel)
    {
        OreType oreType = (OreType)Mathf.RoundToInt(value);

        if (oreType <= OreType.None || oreType >= OreType.Length)
        {
            return "잘못된 광물 티어가 설정되었습니다";
        }

        if (isMaxLevel)
        {
            return $"{GetOreDisplayName(oreType)} 광석 해금 완료";
        }

        return $"{GetOreDisplayName(oreType)} 광석을 해금합니다";
    } // MaxOreTier 설명을 만들어주는 함수.
    // value를 OreType enum 값으로 보고, 해당 광물을 해금하는 설명을 만든다.

    private static string GetOreDisplayName(OreType oreType)
    {
        return oreType switch
        {
            OreType.Copper => "구리", // 사실 0이지만 그냥 해놨음.
            OreType.Iron => "철",
            OreType.Gold => "금",
            OreType.Diamond => "다이아몬드",
            _ => "알 수 없는 광물"
        };
    } // OreType을 UI에 보여줄 이름으로 바꿔주는 함수.

    private static string FormatValue(float value, ValueFormat format)
    {
        return format switch
        {
            ValueFormat.Int => Mathf.RoundToInt(value).ToString(),
            ValueFormat.Percent => $"{value.ToString("0.##")}%",
            ValueFormat.Second => $"{value.ToString("0.##")}초",
            _ => value.ToString("0.##")
        };
    } // 값 뒤에 %, 초 같은 표시 형식을 적용하는 함수.
}