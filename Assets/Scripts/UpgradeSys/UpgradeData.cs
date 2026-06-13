using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드에 들어가는 광물의 재료량을 의미.<br/><br/>
/// 예시)<br/>
/// [업그레이드]<br/>
/// 채굴 속도 + 1<br/>
/// 채굴 범위 + 1<br/>
/// [재료]<br/>
/// 철 - 10개 &lt;&lt;&lt; 이 광물량 하나를 의미.<br/>
/// 구리 - 10개<br/>
/// </summary>
[System.Serializable]
public struct OreAmount
{
    // public OreType oreType;
    public int amount;
    //
    // public OreAmount(OreType oreType, int amount)
    // {
    //     this.oreType = oreType;
    //     this.amount = amount;
    // }
}
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

    public List<OreAmount> baseOreAmounts;
    public float amountMultiplier = 1.2f;

    public List<StatModifier> statModifiers;

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

            //result.Add(new OreAmount(oreAmount.oreType, scaledAmount));
        }
        return result;
    }
}
