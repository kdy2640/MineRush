
public enum OreType
{
    None = -1,
    Copper,
    Iron,
    Gold,
    Diamond,
    Length
}

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
public class OreAmount
{
    public OreType oreType;
    public int amount;

    public OreAmount(OreType oreType, int amount)
    {
        this.oreType = oreType;
        this.amount = amount;
    }
}