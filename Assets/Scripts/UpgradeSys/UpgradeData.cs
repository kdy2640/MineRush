using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 업그레이드 노드를 구성하는 데이터.<br></br>
/// SO이며, 노드 자체를 의미한다긴 보단 노드 뒤에서 주고받는 데이터라고 생각하면 됨.<br></br>
/// 일단 값은 고정될 예정
/// </summary>
[CreateAssetMenu(menuName = "Game/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string id;
    public string displayName;
    [field: SerializeField] public Sprite displayIcon;

    public List<OreAmount> baseOreAmounts;
    public float amountMultiplier = 1.2f;

    public int maxLevel = 1;
    public List<StatModifier> statModifiers;
    public SkillBase skill;
    
    [Header("레벨 기반 광물용(base대신 사용 가능.)")]
    public List<LevelBasedOreCost> levelBasedCosts;
    [Header("일단은 자동채굴용")]
    public List<LevelBasedOreReward> levelBasedRewards;

    /// <summary>
    /// 다음 레벨에 따라 필요한 재료량을 뱉어주는 함수.
    /// </summary>
    /// <param name="level">업그레이드 레벨을 넣어주세요</param>
    /// <returns></returns>
    public List<OreAmount> GetCosts(int level)
    {
        List<OreAmount> result = new();

        foreach (OreAmount oreAmount in baseOreAmounts)
        {
            int scaledAmount = Mathf.RoundToInt(
                oreAmount.amount * Mathf.Pow(amountMultiplier, level)
            );

            result.Add(new OreAmount(oreAmount.oreType, scaledAmount));
        }
        return result;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (StatModifier modifier in statModifiers)
        {
            if (modifier == null)
            {
                continue;
            }

            bool usesOreType =
                modifier.statType == StatType.FragChance ||
                modifier.statType == StatType.PureChance ||
                modifier.statType == StatType.MaxOreTier;

            if (!usesOreType)
            {
                modifier.oreType = OreType.None;
            }

            if (modifier.statType == StatType.MaxOreTier || modifier.statType == StatType.PickaxeTier)
            {
                modifier.modifierType = ModifierType.Max;
            }

            if (modifier.statType != StatType.MaxOreTier)
            {
                continue;
            }

            if (modifier.oreType == OreType.None || modifier.oreType == OreType.Length)
            {
                continue;
            }

            modifier.value = (int)modifier.oreType;
        }
    }
#endif
}
 