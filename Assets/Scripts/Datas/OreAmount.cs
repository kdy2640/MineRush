
public enum OreType
{
    None = -1,
    Cooper,
    Iron,
    Tin,
    Length
}
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